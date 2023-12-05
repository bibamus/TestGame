using System.Threading.Tasks;
using Godot;
using TestGame.world;
using TestGame.world.generation;

namespace TestGame.global;

public partial class Global : Node
{
    
    
    private State _currentState;
    
    private WorldData _worldData;
    
    public override void _Ready()
    {
        base._Ready();
        
        var worldLoadState = new WorldLoadState();
        worldLoadState.WorldLoaded += OnWorldLoaded;
        
        var worldGenerationState = new WorldGenerationState();
        worldGenerationState.WorldGenerated += OnWorldGenerated;
        
        worldLoadState.WorldNotFound += () => SwitchState(worldGenerationState);
        

        SwitchState(worldLoadState);
    }

    private void OnWorldLoaded(WorldData wd)
    {
        _worldData = wd;
        SwitchState(new GameRunState());
    }

    private void OnWorldGenerated(WorldData wd)
    {
        _worldData = wd;
        var error = ResourceSaver.Save( _worldData, "user://world_data.res");
        if (error != Error.Ok)
        {
            GD.PrintErr($"Error saving world data: {error}");
        }
        SwitchState(new GameRunState());
    }

    private void SwitchState(State newState)
    {
        _currentState?.StateExit();
        _currentState = newState;
        _currentState.StateEnter(this);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        _currentState.StateProcess(delta);
        // if (_worldGenerated)
        // {
        //     if (IsInstanceValid(_worldGenerator))
        //     {
        //         _worldGenerator.QueueFree();
        //     }
        // }
    }
}