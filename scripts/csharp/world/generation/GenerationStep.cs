using Godot;

namespace TestGame.world.generation;

[GlobalClass]
public abstract partial class GenerationStep : Node
{
    public virtual WorldData Apply(WorldData worldData, WorldGenerationSettings settings)
    {
        return worldData;
    }
}