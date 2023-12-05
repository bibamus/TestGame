using System.Threading.Tasks;
using Godot;
using TestGame.world;
using TestGame.world.generation;

namespace TestGame.global;

public partial class WorldGenerationState : State
{
    [Signal] public delegate void WorldGeneratedEventHandler(WorldData worldData);
    
    private WorldGenerator _worldGenerator;
    private Task<WorldData> _task;

    public override void StateEnter(Node node)
    {
        _worldGenerator = GD.Load<PackedScene>("res://scenes/world_generator.tscn").Instantiate<WorldGenerator>();
        node.AddChild(_worldGenerator);
        _task = Task.Run(() => _worldGenerator.GenerateWorld(new WorldGenerationSettings()
        {
            WorldWidth = 32000,
            WorldHeight = 32000,
            Seed = 12345
        }));
    }

    public override void StateProcess(double delta)
    {
        if (_task.IsCompleted)
        {
            var worldData = _task.Result;
            GD.Print("Word generation task completed");
            EmitSignal(SignalName.WorldGenerated, worldData);
        }
            
    }

    public override void StateExit()
    {
        _worldGenerator.QueueFree();
    }
}