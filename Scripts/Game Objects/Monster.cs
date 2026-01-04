using Godot;
using System;

public partial class Monster : AnimatedSprite2D {
    public enum MonsterAnimation {
        Attack,
        Death,
        Idle,
        TransformToHuman,
    }
    
    public event Action<MonsterAnimation> FinishedMonsterAnimation;

    private const string Attack = "Attack";
    private const string Death = "Death";
    private const string Idle = "Idle";
    private const string TransformToHuman = "Transform To Human";

    private MonsterAnimation _currentMonsterAnimation = MonsterAnimation.Idle;

    public override void _Ready() {
        AnimationFinished += _HandleAnimationFinished;
    }

    public void PlayAnimation(MonsterAnimation monsterAnimation) {
        _currentMonsterAnimation = monsterAnimation;
        switch (monsterAnimation) {
            case MonsterAnimation.Attack:
                Play(Attack);
                break;
            case MonsterAnimation.Death:
                Play(Death);
                break;
            case MonsterAnimation.Idle:
                Play(Idle);
                break;
            case MonsterAnimation.TransformToHuman:
                Play(TransformToHuman);
                break;
        }
    }

    private void _HandleAnimationFinished() {
        switch (_currentMonsterAnimation) {
            case MonsterAnimation.Attack:
                FinishedMonsterAnimation?.Invoke(_currentMonsterAnimation);
                _currentMonsterAnimation = MonsterAnimation.Idle;
                break;
            case MonsterAnimation.Death:
                FinishedMonsterAnimation?.Invoke(_currentMonsterAnimation);
                break;
            case MonsterAnimation.Idle:
                break;
            case MonsterAnimation.TransformToHuman:
                break;
        }
    }
}