using Godot;
using System;
using System.Diagnostics;

public partial class MainCharacter : Character {


	Hitbox leftFist;
	Hitbox rightFist;
	iGrabbable isGrabbing;
	AnimationPlayer _animationPlayer;
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
		base.SwitchState(state);
		if (state == CharacterState.Idle) {
			leftFist.disable();
			rightFist.disable();
		}
		
	}

	protected override void onCollide(KinematicCollision2D collision)
	{
		GodotObject body2D = collision.GetCollider();
		if (body2D is IHurtbox) {
			if (body2D is TileMapLayer) {
				TileMapLayer mapLayer= (TileMapLayer)body2D;
				Vector2 collisionPos = collision.GetPosition() + velocity.Normalized() * 5f; /// add a slight offset to gurantee position
				Vector2I tileCoords = mapLayer.LocalToMap(collisionPos);
				mapLayer.EraseCell(tileCoords); // 0 is layer index
			}
		}
	}

	protected override void runPostPhysics(double delta)
	{
		base.runPostPhysics(delta);
		GD.Print(poise);
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
		if (this.isGrabbing != null) {
			_animationPlayer.ClearQueue();
			_animationPlayer.Play("right_grab");
			this.SwitchState(CharacterState.AttackingV2);
			this.isGrabbing.Throw(GetGlobalMousePosition()); /// apparently we face our butt to the enemy

			this.isGrabbing = null;
		}
		else {
			_animationPlayer.ClearQueue();
			_animationPlayer.Play("right_grab");
			this.SwitchState(CharacterState.AttackingV2);
			rightFist.enable();
		}
		
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
		
		if (this.isGrabbing == null) {
			this.SwitchState(CharacterState.Grabbing);
		}
		else {
			this.SwitchState(CharacterState.Idle);
		}
	}

	private void onLeftHit(Node node) {
		if (_state != CharacterState.AttackingV1 || this == node || node == null) {
			return;
		}
		if (node is not IHurtbox && node is not IHitbox) {
			onAnimationEnd("left_punch");
			return;
		}
		
		if (node is IHitbox) {
			return;
		}

		Node owner = (node as IHurtbox).getHurtboxOwner();
		if (owner == this) {
			return;
		}
		if (owner == null) {
			onAnimationEnd("left_punch");
			return;
		}


		if (owner is iPoise) {
			(owner as iPoise).addPoise(30f);
		}

		onAnimationEnd("left_punch");
		return;

	}

	private void onRightGrab(Node node) {
		if (_state != CharacterState.AttackingV2 || this == node || node == null) {
			return;
		}

		if (this == node) {
			return;
		}
		

	
		Node owner = null;
		
		if (node is IHurtbox) {
			owner = (node as IHurtbox).getHurtboxOwner();
		}
		else if (node is iGrabbable) {
			owner = node;
		}

		if (owner == null || owner == this) {
			return;
		}

		if (owner is iGrabbable && (owner as iGrabbable).IGrabbable()) {
			(owner as iGrabbable).Grab(rightFist);
			this.isGrabbing = (iGrabbable)owner;
		}

		onAnimationEnd("right_grab");
	}
}
