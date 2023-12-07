using System.Threading.Tasks;
using Godot;
using TestGame.world;
using TestGame.world.generation;

namespace TestGame.global;

public partial class Global : Node
{
    private State _currentState;

    public WorldData WorldData { get; private set; }

    private WorldInitializeState _worldInitializeState;
    
    [Export] private int _worldWidth = 1024;
    [Export] private int _worldHeight = 1024;

    public override void _Ready()
    {
        base._Ready();

        var worldLoadState = new WorldLoadState();
        worldLoadState.WorldLoaded += OnWorldLoaded;

        var worldGenerationState = new WorldGenerationState(new WorldGenerationSettings
        {
            WorldWidth = _worldWidth,
            WorldHeight = _worldHeight,
            Seed = 12345
            
        });
        worldGenerationState.WorldGenerated += OnWorldGenerated;

        worldLoadState.WorldNotFound += () => SwitchState(worldGenerationState);

        _worldInitializeState = new WorldInitializeState(this);

        _worldInitializeState.WorldInitializedFinished += () => SwitchState(new GameRunState());

        SwitchState(worldLoadState);
    }

    private void OnWorldLoaded(WorldData wd)
    {
        WorldData = wd;
        SwitchState(_worldInitializeState);
    }

    private void OnWorldGenerated(WorldData wd)
    {
        WorldData = wd;
        ResourceSaver.Save(wd, "user://world_data.res");
        SwitchState(_worldInitializeState);
    }

    private void SwitchState(State newState)
    {
        _currentState?.StateExit();
        _currentState?.QueueFree();
        _currentState = newState;
        _currentState.StateEnter();
        _currentState.Name = newState.GetType().Name;
        AddChild(_currentState);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        _currentState.StateProcess(delta);
    }
}