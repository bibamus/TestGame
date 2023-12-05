using System;
using Godot;

namespace TestGame.world;

public partial class WorldData : Resource
{
    public WorldData(int worldWidth, int worldHeight)
    {
        ValidateInput(worldWidth, worldHeight);

        WorldWidth = worldWidth;
        WorldHeight = worldHeight;
        Blocks = new Block[WorldWidth, WorldHeight];

        HChunkCount = (int) Math.Ceiling((double) WorldWidth / ChunkSize.X);
        VChunkCount = (int) Math.Ceiling((double) WorldHeight / ChunkSize.Y);
        GD.Print($"World has {HChunkCount} horizontal chunks and {VChunkCount} vertical chunks");
    }

    // ReSharper disable once UnusedMember.Global - Used by ResourceLoader
    public WorldData()
    {
    }

    public Vector2I ChunkSize { get; } = new Vector2I(64, 64);

    public int HChunkCount { get; private set; }
    public int VChunkCount { get; private set; }

    public int WorldWidth { get; }
    public int WorldHeight { get; }
    public int SurfaceLevel { get; set; }
    public Block[,] Blocks { get; set; }

    public Vector2I BottomLeft => new Vector2I(-WorldWidth / 2, WorldHeight / 2);
    public Vector2I BottomRight => new Vector2I(WorldWidth / 2, WorldHeight / 2);
    public Vector2I TopLeft => new Vector2I(-WorldWidth / 2, -WorldHeight / 2);
    public Vector2I TopRight => new Vector2I(WorldWidth / 2, -WorldHeight / 2);


    public Block[,] GetChunk(int chunkX, int chunkY)
    {
        if (chunkX < 0 || chunkX >= HChunkCount || chunkY < 0 || chunkY >= VChunkCount)
        {
            GD.PrintErr($"Chunk at position {chunkX},{chunkY} is out of bounds");
            return null;
        }

        var chunk = new Block[ChunkSize.X, ChunkSize.Y];
        for (var x = 0; x < ChunkSize.X; x++)
        {
            for (var y = 0; y < ChunkSize.Y; y++)
            {
                var worldX = chunkX * ChunkSize.X + x;
                var worldY = chunkY * ChunkSize.Y + y;
                chunk[x, y] = Blocks[worldX, worldY];
            }
        }

        return chunk;
    }

    private static void ValidateInput(int worldWidth, int worldHeight)
    {
        if (worldWidth <= 0)
        {
            throw new ArgumentException("World width must be greater than 0");
        }

        if (worldHeight <= 0)
        {
            throw new ArgumentException("World height must be greater than 0");
        }

        if (worldWidth % 2 != 0)
        {
            throw new ArgumentException("World width must be even");
        }

        if (worldHeight % 2 != 0)
        {
            throw new ArgumentException("World height must be even");
        }
    }

    public Block GetBlockAtWorldPosition(Vector2I worldPosition)
    {
        var x = worldPosition.X + WorldWidth / 2;
        var y = -worldPosition.Y + WorldHeight / 2;
        if (x < 0 || x >= WorldWidth || y < 0 || y >= WorldHeight)
        {
            GD.PrintErr($"Block at position {worldPosition} is out of bounds");
            return null;
        }

        return Blocks[x, y];
    }
}