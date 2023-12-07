using System.Threading.Tasks;
using Godot;
using TestGame.world;
using TestGame.world.generation;

namespace TestGame.global;

public partial class WorldGenerationState : State
{
    [Signal]
    public delegate void WorldGeneratedEventHandler(WorldData worldData);

    private WorldGenerator _worldGenerator;
    private WorldGenerationSettings _settings;

    public WorldGenerationState(WorldGenerationSettings settings)
    {
        _settings = settings;
    }


    public override void StateEnter()
    {
        _worldGenerator = GD.Load<PackedScene>("res://scenes/world_generator.tscn").Instantiate<WorldGenerator>();
        _worldGenerator.Settings = _settings;
        _worldGenerator.GenerationStepFinished += (stepName) => GD.Print($"World generation finished step {stepName}");
        _worldGenerator.WorldGenerated += (worldData) => EmitSignal(SignalName.WorldGenerated, worldData);
        AddChild(_worldGenerator);
    }

    public override void StateProcess(double delta)
    {

    }

    public override void StateExit()
    {
        _worldGenerator.QueueFree();
    }
}