using System.Collections.Generic;
using Godot;
using InputSystem;
using ServiceSystem;

public partial class Player : AnimatedSprite2D, IInputState, ITick {
    private const string AttackOne = "Attack One";
    private const string Move = "Move";
    private const string Idle = "Idle";

    private GameClock _gameClock;
    private float _movementSpeed = 180 / Engine.PhysicsTicksPerSecond;

    public override void _Ready() {
        ServiceLocator serviceLocator = GetNode<ServiceLocator>(ServiceLocator.AutoloadPath);
        _gameClock = serviceLocator.GetService<GameClock>(ServiceName.GameClock);
        _gameClock.AddActiveScene(this, GetInstanceId());
    }

    public override void _ExitTree() {
        _gameClock.RemoveActiveScene(GetInstanceId());
    }

    public void ProcessInput(InputEventDto eventDto) { }

    public void PhysicsTick() {
        int movementX = 0;
        if (Input.IsActionPressed("MoveLeft")) {
            movementX -= 1;
        }

        if (Input.IsActionPressed("MoveRight")) {
            movementX += 1;
        }

        if (movementX == 0) {
            Play(Idle);
            return;
        }

        Play(Move);
        FlipH = movementX < 0;
        Position += new Vector2(movementX * _movementSpeed, 0);
    }
}