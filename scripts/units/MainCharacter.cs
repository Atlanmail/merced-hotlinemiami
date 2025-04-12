using Godot;
using System;

public partial class MainCharacter : Character {

    AnimationPlayer _animationPlayer;

    Area2D leftFist;
	Area2D rightFist;
    public override void _Ready()
	{
		base._Ready();

        leftFist = (Area2D)NodeExtensions.FindFirstChildOfName(this, "LeftFist");
        rightFist = (Area2D)NodeExtensions.FindFirstChildOfName(this, "RightFist");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

	}

    public void left_action()
    {
        _animationPlayer.ClearQueue();
        _animationPlayer.Play("left_punch");

        
        
    }

    public void right_action()
    {
        
        _animationPlayer.ClearQueue();
        _animationPlayer.Play("right_grab");
    }
}