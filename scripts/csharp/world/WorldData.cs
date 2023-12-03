using Godot;

namespace TestGame.world;

public partial class WorldData : Resource
{
    public WorldData(int worldWidth, int worldHeight)
    {
        WorldWidth = worldWidth;
        WorldHeight = worldHeight;
        Blocks = new Block[WorldWidth, WorldHeight];
    }

    public int WorldWidth { get; }
    public int WorldHeight { get;  }
    public int SurfaceLevel { get; set; }
    public Block[,] Blocks { get; }
}