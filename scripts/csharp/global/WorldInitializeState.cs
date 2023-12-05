using System;
using System.Threading.Tasks;
using Godot;
using TestGame.world;

namespace TestGame.global;

public partial class WorldInitializeState : State
{
    
    [Signal] public delegate void WorldInitializedFinishedEventHandler();
    
    private readonly Global _globalNode;
    private Task _task;
    private World _world;

    public WorldInitializeState(Global globalNode)
    {
        _globalNode = globalNode;
    }
    public override void StateEnter()
    {
        base.StateEnter();
        
        var worldData = _globalNode.WorldData; 
        _world = GD.Load<PackedScene>("res://scenes/world.tscn").Instantiate<World>();
        _task = Task.Run(() => _world.Init(worldData));
    }
    
    public override void StateProcess(double delta)
    {
        if (_task.IsCompleted)
        {
            GD.Print("World initialization task completed");
            GetTree().Root.AddChild(_world);
            _task.Dispose();
            EmitSignal(SignalName.WorldInitializedFinished);
        }
    }
    
}