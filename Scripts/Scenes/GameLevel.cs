using Godot;
using ServiceSystem;

public partial class GameLevel : Node2D, ITick {
    [Export]
    private SceneId _sceneId;

    [Export]
    private Area2D _triggerCombatArea;

    [Export]
    private Player _player;

    [Export]
    private Button _buttonAttackOne;

    [Export]
    private Button _buttonAttackTwo;

    [Export]
    private Button _buttonAttackThree;

    [Export]
    private Button _buttonAttackFour;

    [Export]
    private Control _combatUi;

    private GameClock _gameClock;
    private SceneManager _sceneManager;
    private int _ticksPerStage = 20 * Engine.PhysicsTicksPerSecond;

    public override void _Ready() {
        ServiceLocator serviceLocator = GetNode<ServiceLocator>(ServiceLocator.AutoloadPath);
        _gameClock = serviceLocator.GetService<GameClock>(ServiceName.GameClock);
        _sceneManager = serviceLocator.GetService<SceneManager>(ServiceName.SceneManager);
        _sceneManager.SetCurrentSceneId(_sceneId);

        _gameClock.AddActiveScene(this, GetInstanceId());

        _triggerCombatArea.AreaEntered += _TriggerCombat;
        _buttonAttackOne.Pressed += () => _Attack(Player.AttackType.One);
        _buttonAttackTwo.Pressed += () => _Attack(Player.AttackType.Two);
        _buttonAttackThree.Pressed += () => _Attack(Player.AttackType.Three);

        _combatUi.Visible = false;
    }


    public override void _ExitTree() {
        _gameClock.RemoveActiveScene(GetInstanceId());
    }

    public void PhysicsTick() { }

    private void _TriggerCombat(Area2D area) {
        if (area.GetParent() is Player) {
            GD.Print("Player entered combat");
            _player.SetState(Player.PlayerState.Combat);
            _combatUi.Visible = true;
        }
    }

    private void _Attack(Player.AttackType attackType) {
        _player.Attack(attackType);
    }
}