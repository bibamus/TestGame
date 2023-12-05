using Godot;
using TestGame.world;

namespace TestGame.global;

public partial class WorldLoadState : State
{
    [Signal]
    public delegate void WorldLoadedEventHandler(WorldData worldData);

    [Signal]
    public delegate void WorldNotFoundEventHandler();

    public override void StateProcess(double delta)
    {
        base.StateProcess(delta);
        if (ResourceLoader.Exists("user://world_data.res"))
        {
            var worldData = ResourceLoader.Load<WorldData>("user://world_data.res");
            GD.Print("World data loaded");
            EmitSignal(SignalName.WorldLoaded, worldData);
        }
        else
        {
            GD.Print("World data not found");
            EmitSignal(SignalName.WorldNotFound);
        }
    }
}