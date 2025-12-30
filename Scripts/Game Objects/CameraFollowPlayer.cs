using Godot;
using System;

public partial class CameraFollowPlayer : Camera2D {
    [Export]
    private Player _player;

    public override void _PhysicsProcess(double delta) {
        Position = new Vector2(_player.Position.X, Position.Y);
    }
}