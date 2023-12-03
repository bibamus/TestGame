using Godot;

namespace TestGame.world.generation;

[GlobalClass]
public abstract partial class GenerationStep : Node
{
    public abstract WorldGenerationData Apply(WorldGenerationData data, WorldGenerationSettings settings);
}