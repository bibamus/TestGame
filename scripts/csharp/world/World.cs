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
        var surroundingChunks = GetSurroundingChunks(playerChunk);
        var children = GetChildren()
            .Where(node => node is TileMapChunk)
            .Cast<TileMapChunk>()
            .ToList();

        var goal = new List<TileMapChunk>();

        foreach (var chunk in surroundingChunks)
        {
            var chunkX = chunk.X;
            var chunkY = chunk.Y;
            if (chunkX < 0 || chunkX >= _worldData.HChunkCount || chunkY < 0 || chunkY >= _worldData.VChunkCount)
            {
                continue;
            }

            var chunkNode = _chunkMaps[chunkX, chunkY];
            if (chunkNode == null)
            {
                continue;
            }

            goal.Add(chunkNode);
        }

        foreach (var child in children)
        {
            if (!goal.Contains(child))
            {
                RemoveChild(child);
            }
        }

        foreach (var chunk in goal)
        {
            if (chunk.GetParent() == this)
            {
                continue;
            }

            AddChild(chunk);
        }
    }

    private Vector2I GetPlayerChunk()
    {
        var playerPos = Player.Position;
        var chunkX = (int) Math.Floor(playerPos.X / (_worldData.ChunkSize.X * 16));
        var chunkY = (int) Math.Floor(playerPos.Y / (_worldData.ChunkSize.Y * 16));
        return new Vector2I(chunkX, chunkY);
    }

    private Vector2I[] GetSurroundingChunks(Vector2I chunk)
    {
        var chunks = new Vector2I[9];
        var index = 0;
        for (var x = chunk.X - 1; x <= chunk.X + 1; x++)
        {
            for (var y = chunk.Y - 1; y <= chunk.Y + 1; y++)
            {
                chunks[index] = new Vector2I(x, y);
                index++;
            }
        }

        return chunks;
    }

    public void Init(WorldData worldData)
    {
        _worldData = worldData;
        var tileMapScene = GD.Load<PackedScene>("res://scenes/tile_map.tscn");
        _chunkMaps = new TileMapChunk[_worldData.HChunkCount, _worldData.VChunkCount];

        GD.Print("Generating chunk maps");
        for (var x = 0; x < _worldData.HChunkCount; x++)
        {
            for (var y = 0; y < _worldData.VChunkCount; y++)
            {
                var chunk = _worldData.GetChunk(x, y);
                var chunkNode = tileMapScene.Instantiate<TileMapChunk>();
                chunkNode.Name = $"Chunk {x},{y}";
                chunkNode.Blocks = chunk;
                var chunkY = (_worldData.VChunkCount - y) * _worldData.ChunkSize.Y * 16;
                var chunkX = x * _worldData.ChunkSize.X * 16;
                chunkNode.Position = new Vector2(chunkX, chunkY);
                _chunkMaps[x, y] = chunkNode;
            }
        }

        GD.Print("Finished generating chunk maps");
    }
}