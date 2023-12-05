using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using TestGame.world.generation;

namespace TestGame.world;

public partial class World : Node
{
    [Signal]
    public delegate void WorldLoadedEventHandler();

    [Export] public Node2D Player { get; set; }

    private WorldData _worldData;

    private TileMapChunk[,] _chunkMaps;

    private Vector2I _chunkSize = new Vector2I(64, 64);
    private int _hChunkCount;
    private int _vChunkCount;

    private Vector2I _playerChunk;


    public override void _Ready()
    {
        base._Ready();
        UpdateChunks();
    }


    public override void _Process(double delta)
    {
        base._Process(delta);
        UpdateChunks();
    }

    private void UpdateChunks()
    {
        var playerChunk = GetPlayerChunk();
        if (playerChunk == _playerChunk) return;

        var surroundingChunks = GetSurroundingChunks(playerChunk)
            .Select(coords => _chunkMaps[coords.X, coords.Y])
            .ToHashSet();
        
        var children = GetChildren()
            .Where(node => node is TileMapChunk)
            .Cast<TileMapChunk>()
            .ToList();

        foreach (var existing in children)
        {
            if (!surroundingChunks.Contains(existing))
            {
                RemoveChild(existing);
            }
        }

        foreach (var chunk in surroundingChunks)
        {
            if (chunk.GetParent() == this)
            {
                continue;
            }

            AddChild(chunk);
        }

        _playerChunk = playerChunk;
    }

    private Vector2I GetPlayerChunk()
    {
        var playerPos = Player.Position;
        var chunkX = (int) Math.Floor(playerPos.X / (_chunkSize.X * 16));
        var chunkY = (int) Math.Floor(playerPos.Y / (_chunkSize.Y * 16));
        return new Vector2I(chunkX, chunkY);
    }

    private IEnumerable<Vector2I> GetSurroundingChunks(Vector2I chunk)
    {
        var chunks = new List<Vector2I>();
        for (var x = Math.Max(chunk.X - 1, 0); x <= Math.Min(chunk.X + 1, _hChunkCount - 1); x++)
        {
            for (var y = Math.Max(chunk.Y - 1, 0); y <= Math.Min(chunk.Y + 1, _vChunkCount - 1); y++)
            {
                chunks.Add(new Vector2I(x, y));
            }
        }

        return chunks;
    }

    public void Init(WorldData worldData)
    {
        _worldData = worldData;
        var tileMapScene = GD.Load<PackedScene>("res://scenes/tile_map.tscn");
        _hChunkCount = (int) Math.Ceiling((float) _worldData.Width / _chunkSize.X);
        _vChunkCount = (int) Math.Ceiling((float) _worldData.Height / _chunkSize.Y);

        _chunkMaps = new TileMapChunk[_hChunkCount, _vChunkCount];

        GD.Print("Generating chunk maps");
        for (var x = 0; x < _hChunkCount; x++)
        {
            for (var y = 0; y < _vChunkCount; y++)
            {
                var chunk = GetChunk(x, y);
                var chunkNode = tileMapScene.Instantiate<TileMapChunk>();
                chunkNode.Name = $"Chunk {x},{y}";
                chunkNode.ChunkBlocks = chunk;
                var chunkY = (_vChunkCount - y) * _chunkSize.Y * 16;
                var chunkX = x * _chunkSize.X * 16;
                chunkNode.Position = new Vector2(chunkX, chunkY);
                _chunkMaps[x, y] = chunkNode;
            }
        }

        GD.Print("Finished generating chunk maps");
    }

    private BlockType[,] GetChunk(int x, int y)
    {
        var chunk = new BlockType[_chunkSize.X, _chunkSize.Y];
        for (var chunkX = 0; chunkX < _chunkSize.X; chunkX++)
        {
            for (var chunkY = 0; chunkY < _chunkSize.Y; chunkY++)
            {
                var worldX = x * _chunkSize.X + chunkX;
                var worldY = y * _chunkSize.Y + chunkY;
                chunk[chunkX, chunkY] = _worldData.Blocks[worldX][worldY];
            }
        }

        return chunk;
    }
}