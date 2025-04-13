using Godot;
using System;
using System.Diagnostics;

public partial class MainCharacter : Character {

    AnimationPlayer _animationPlayer;

    Hitbox leftFist;
	Hitbox rightFist;
    public override void _Ready()
	{
		base._Ready();

        leftFist = (Hitbox)NodeExtensions.FindFirstChildOfName(this, "LeftFist");
        rightFist = (Hitbox)NodeExtensions.FindFirstChildOfName(this, "RightFist");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _animationPlayer.AnimationFinished += this.onAnimationEnd;
        leftFist.HitboxStruck += this.onLeftHit;
        rightFist.HitboxStruck += this.onRightGrab;
	}

    protected override void SwitchState(CharacterState state) {
        this._state = state;

        if (state == CharacterState.Idle) {
            leftFist.disable();
            rightFist.disable();
        }
        
    }
    

    public void left_action()
    {
        _animationPlayer.ClearQueue();
        _animationPlayer.Play("left_punch"); 
        this.SwitchState(CharacterState.AttackingV1);
        leftFist.enable();

    }

    public void right_action()
    {
        _animationPlayer.ClearQueue();
        _animationPlayer.Play("right_grab");
    }
    private void onAnimationEnd(StringName myString) {

        if (myString == "left_punch") {
            leftFist.disable();
        }
        if (myString == "right_grab") {
            rightFist.disable();
        }

        _animationPlayer.ClearQueue();
        _animationPlayer.Play("RESET");
        this.SwitchState(CharacterState.Idle);
    }

    private void onLeftHit(Node node) {
        if (_state != CharacterState.AttackingV1) {
            return;
        }
        if (node is not IHurtbox) {
            onAnimationEnd("left_punch");
            return;
        }

        Node owner = (node as IHurtbox).getHurtboxOwner();

        if (owner == null) {
            onAnimationEnd("left_punch");
            return;
        }

        if (owner is iPoise) {
            (owner as iPoise).addPoise(0.3f);
            GD.Print("Hit owner!");
        }

        onAnimationEnd("left_punch");
        return;

    }

    private void onRightGrab(Node node) {
        throw new NotImplementedException("Right grab not implemented yet");
    }
}