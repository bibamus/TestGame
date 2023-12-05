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

    public override void _Ready()
    {
        base._Ready();
        
        var worldLoadState = new WorldLoadState();
        worldLoadState.WorldLoaded += OnWorldLoaded;
        
        var worldGenerationState = new WorldGenerationState();
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
        SwitchState(_worldInitializeState);
    }

    private void SwitchState(State newState)
    {
        _currentState?.StateExit();
        _currentState?.QueueFree();
        _currentState = newState;
        _currentState.StateEnter();
        AddChild(_currentState);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        _currentState.StateProcess(delta);
    }
}