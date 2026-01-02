using Godot;
using InputSystem;
using ServiceSystem;

public partial class Player : AnimatedSprite2D, IInputState, ITick {
    public enum PlayerState {
        Moving,
        Combat
    }

    public enum AttackType {
        One,
        Two,
        Three,
        Four
    }

    private const string AttackOne = "Attack One";
    private const string AttackTwo = "Attack Two";
    private const string AttackThree = "Attack Three";
    private const string AttackFour = "Attack Four";
    private const string Move = "Move";
    private const string Idle = "Idle";

    private GameClock _gameClock;
    private float _movementSpeed = 180 / Engine.PhysicsTicksPerSecond;
    private PlayerState _state = PlayerState.Moving;

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
        switch (_state) {
            case PlayerState.Moving:
                _Moving();
                break;
            case PlayerState.Combat:
                _Combat();
                break;
        }
    }

    public void Attack(AttackType type) {
        GD.Print($"Attack type {type}");
        switch (type) {
            case AttackType.One:
                Play(AttackOne);
                break;
            case AttackType.Two:
                Play(AttackTwo);
                break;
            case AttackType.Three:
                Play(AttackThree);
                break;
            case AttackType.Four:
                break;
        }
    }

    public void SetState(PlayerState newState) {
        _state = newState;

        switch (_state) {
            case PlayerState.Combat:
                Play(Idle);
                break;
        }
    }

    private void _Moving() {
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

    private void _Combat() { }
}