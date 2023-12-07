using Godot;

namespace TestGame.world.generation;

public partial class WorldGenerationSettings : Resource
{
    public int WorldWidth { get; set; }
    public int WorldHeight { get; set; }
    
    public int Seed { get; set; }
}