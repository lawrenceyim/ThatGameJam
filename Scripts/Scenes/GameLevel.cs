using Godot;
using ServiceSystem;

public partial class GameLevel : Node2D, ITick {
    public enum CombatTurn {
        Player,
        PlayerAnimation,
        Monster,
        MonsterAnimation,
    }

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
    private Label _buttonFourLabel;

    [Export]
    private Control _combatUi;

    [Export]
    private Monster _monster;

    private GameClock _gameClock;
    private SceneManager _sceneManager;
    private int _ticksPerStage = 20 * Engine.PhysicsTicksPerSecond;
    private CombatTurn _combatTurn = CombatTurn.Player;
    private bool _inCombat = false;
    private int _playerHealth = 3;
    private int _monsterHealth = 3;

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
        _buttonAttackFour.Pressed += () => _Attack(Player.AttackType.Four);

        _player.FinishedPlayerAnimation += _HandlePlayerAnimationFinished;
        _monster.FinishedMonsterAnimation += HandleMonsterMonsterAnimationFinishedMonster;

        _combatUi.Visible = false;
        _buttonFourLabel.Text = GlobalSettings.SecretAttackUnlocked ? "4" : "?";
    }

    public override void _Process(double delta) {
        _CombatInput();
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
            _inCombat = true;
        }
    }

    private void _CombatInput() {
        if (!_inCombat) {
            return;
        }

        if (Input.IsActionJustPressed(InputConst.AttackOne)) {
            GD.Print("Attack one pressed");
            _Attack(Player.AttackType.One);
        } else if (Input.IsActionJustPressed(InputConst.AttackTwo)) {
            GD.Print("Attack two pressed");
            _Attack(Player.AttackType.Two);
        } else if (Input.IsActionJustPressed(InputConst.AttackThree)) {
            GD.Print("Attack three pressed");
            _Attack(Player.AttackType.Three);
        } else if (Input.IsActionJustPressed(InputConst.AttackFour)) {
            GD.Print("Attack four pressed");
            _Attack(Player.AttackType.Four);
        }
    }

    private void _Attack(Player.AttackType attackType) {
        if (_combatTurn != CombatTurn.Player) {
            return;
        }

        if (attackType == Player.AttackType.Four && !GlobalSettings.SecretAttackUnlocked) {
            return;
        }

        _player.Attack(attackType);
        _combatTurn = CombatTurn.PlayerAnimation;
    }

    private void _HandlePlayerAnimationFinished(Player.PlayerAnimation animation) {
        switch (animation) {
            case Player.PlayerAnimation.Attack:
                _MonsterTakeDamage();
                _player.PlayAnimation(Player.PlayerAnimation.Idle);

                if (_monsterHealth == 0) {
                    return;
                }

                _MonsterAttack();
                break;
            case Player.PlayerAnimation.Death:
                _PlayerDie();
                break;
            case Player.PlayerAnimation.SpecialAttack:
                _monster.PlayAnimation(Monster.MonsterAnimation.TransformToHuman);
                break;
        }
    }

    private void _MonsterAttack() {
        _combatTurn = CombatTurn.Monster;
        _monster.PlayAnimation(Monster.MonsterAnimation.Attack);
        _combatTurn = CombatTurn.MonsterAnimation;
    }

    private void HandleMonsterMonsterAnimationFinishedMonster(Monster.MonsterAnimation monsterAnimation) {
        switch (monsterAnimation) {
            case Monster.MonsterAnimation.Attack:
                _PlayerTakeDamage();
                _combatTurn = CombatTurn.Player;
                break;
            case Monster.MonsterAnimation.Death:
                _MonsterDie();
                break;
        }
    }

    private void _PlayerTakeDamage() {
        _playerHealth--;

        if (_playerHealth == 0) {
            _player.PlayAnimation(Player.PlayerAnimation.Death);
        }
    }

    private void _MonsterTakeDamage() {
        _monsterHealth--;
        if (_monsterHealth == 0) {
            _monster.PlayAnimation(Monster.MonsterAnimation.Death);
        }
    }

    private void _PlayerDie() {
        _sceneManager.ChangeToCurrentScene();
    }

    private void _MonsterDie() {
        GlobalSettings.SecretAttackUnlocked = true;
        _sceneManager.ChangeToCurrentScene();
    }
}