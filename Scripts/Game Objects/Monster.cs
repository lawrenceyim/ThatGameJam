using Godot;
using System;

public partial class Monster : AnimatedSprite2D {
    public enum Animation {
        Attack,
        Death,
        Idle,
        TransformToHuman,
    }

    private const string Attack = "Attack";
    private const string Death = "Death";
    private const string Idle = "Idle";
    private const string TransformToHuman = "TransformToHuman";

    public void PlayAnimation(Animation animation) {
        switch (animation) {
            case Animation.Attack:
                Play(Attack);
                break;
            case Animation.Death:
                Play(Death);
                break;
            case Animation.Idle:
                Play(Idle);
                break;
            case Animation.TransformToHuman:
                Play(TransformToHuman);
                break;
        }
    }
}