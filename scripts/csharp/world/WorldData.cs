using Godot;
using Godot.Collections;

namespace TestGame.world;

public partial class WorldData : Resource
{
    [Export] public Array<Array<BlockType>> Blocks { get; set; }
    [Export] public int Width { get; set; }
    [Export] public int Height { get; set; }
    [Export] public int SurfaceLevel { get; set; }
}