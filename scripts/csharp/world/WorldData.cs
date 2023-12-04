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
    }

    private static void ValidateInput(int worldWidth, int worldHeight)
    {
        if (worldWidth <= 0)
        {
            throw new System.ArgumentException("World width must be greater than 0");
        }

        if (worldHeight <= 0)
        {
            throw new System.ArgumentException("World height must be greater than 0");
        }

        if (worldWidth % 2 != 0)
        {
            throw new System.ArgumentException("World width must be even");
        }

        if (worldHeight % 2 != 0)
        {
            throw new System.ArgumentException("World height must be even");
        }
    }

    public int WorldWidth { get; }
    public int WorldHeight { get; }
    public int SurfaceLevel { get; set; }
    public Block[,] Blocks { get; }

    public Vector2I BottomLeft => new Vector2I(-WorldWidth / 2, WorldHeight / 2);
    public Vector2I BottomRight => new Vector2I(WorldWidth / 2, WorldHeight / 2);
    public Vector2I TopLeft => new Vector2I(-WorldWidth / 2, -WorldHeight / 2);
    public Vector2I TopRight => new Vector2I(WorldWidth / 2, -WorldHeight / 2);
    
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