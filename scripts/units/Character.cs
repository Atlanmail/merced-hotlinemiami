using Godot;
using System;
using System.Collections.Generic;

public partial class Character : CharacterBody2D, iMove, iPoise, IHurtbox
{

	protected enum CharacterState
	{
		Idle,
		Walking,
		AttackingV1,
		AttackingV2,
		Charging
	}


	#region Variables
	[Export]
	private int speed = 300;
	
	protected CharacterState _state = CharacterState.Idle;

	[Export]
	private float poise = 100; 
	[Export]
	private float poiseDecayRate = 0; /// how much the health decays per second
	
	private Vector2 moveDir = new Vector2(0,0);
	private Vector2 faceAtPoint;
	CharacterBody2D characterBody2D;
	
	List<IHurtbox> hurtboxes = new List<IHurtbox>();
	bool hurtboxEnabled = false;

	#endregion

	protected virtual void SwitchState(CharacterState state) {

	}



	public override void _Ready()
	{
		base._Ready();
		faceAtPoint = Position + Vector2.Up* 3;
	}


	#region IMove
	public int getSpeed()
	{
		return speed;
	}

	public void setSpeed(int speed) {
		this.speed = speed;
	}
	/**
	Moves towards a global position
	*/
	public void move(float x, float y)
	{
		moveDir = new Vector2(x,y) - Position;
		moveDir = moveDir.Normalized();
	}

	public void face(Vector2 globalPosition) {
		faceAtPoint = globalPosition;
	}

	public void shove(int x, int y, float force)
	{
		throw new NotImplementedException();
	}
	#endregion IMove

	public override void _PhysicsProcess(double delta)
	{
		///GD.Print(moveDir);
		KinematicCollision2D collision = MoveAndCollide( moveDir * (float)speed * (float)delta);
		LookAt(faceAtPoint);
		this.Rotation = this.Rotation + Mathf.DegToRad(90);

		/// handle collisions
		;

		if (collision != null) {
			GodotObject body2D = collision.GetCollider();
			if (body2D is IHurtbox) {
				TileMapLayer mapLayer= (TileMapLayer)body2D;
				Vector2 collisionPos = collision.GetPosition() + moveDir * 5f; /// add a slight offset to gurantee position
				Vector2I tileCoords = mapLayer.LocalToMap(collisionPos);

				mapLayer.EraseCell(tileCoords); // 0 is layer index
			}
		}

		
	   

	}

	#region IPoise
	public float getPoise()
	{
		return poise;
	}

	public void setPoise(float poise)
	{
		this.poise = poise;
	}

	public void decayPoise()
	{
		throw new NotImplementedException();
	}
	
	public void addPoise(float poise) {
		this.poise += poise;
		GD.Print("Added poise");
		this.poise = Mathf.Clamp(this.poise, 0,1);
	}
	#endregion

	#region IHurtbox
	public Node getHurtboxOwner()
	{
		return this;
	}

	public void enable()
	{	
		this.hurtboxEnabled = true;
		foreach (IHurtbox hurtbox in this.hurtboxes) {
			hurtbox.enable();
		}
	}

	public void disable()
	{
		this.hurtboxEnabled = false;
		foreach (IHurtbox hurtbox in this.hurtboxes) {
			hurtbox.disable();
		}
	}

	public bool isEnabled()
	{
		return this.hurtboxEnabled;
	}
	#endregion



}
