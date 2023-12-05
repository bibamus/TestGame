using Godot;

namespace TestGame.global;

public partial class GameRunState : State
{
    private bool _initialized = false;

    public override void StateEnter()
    {
    }

    public override void StateProcess(double delta)
    {
        base.StateProcess(delta);
        if (!_initialized)
        {
            var player = GD.Load<PackedScene>("res://scenes/player.tscn").Instantiate<Node2D>();
            player.Position = new Vector2(80, 80);
            GetTree().Root.AddChild(player);
            _initialized = true;
        }
    }
}