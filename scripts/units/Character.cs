using Godot;
using System;
using System.Collections.Generic;

public partial class Character : CharacterBody2D, iMove, iPoise, IHitboxOwner
{

	protected enum CharacterState
	{
		Idle,
		Unsteady,
		Walking,
		AttackingV1,
		AttackingV2,
		Charging,
		Grabbing,
		Grabbed
	}


	#region Variables
	[Export]
	private int speed = 300;
	
	protected CharacterState _state = CharacterState.Idle;

	[Export]
	private float MAX_POISE = 100f;
	private float poise = 0; 
	[Export]
	private float POISE_DECAYRATE = 0; /// how much the health decays per second
	
	private Vector2 moveDir = new Vector2(0,0);
	private Vector2 faceAtPoint;
	
	List<IHurtbox> hurtboxes = new List<IHurtbox>();
	bool hurtboxEnabled = false;

	private Vector2 _velocity;
	protected Vector2 velocity {
		get { return _velocity; }
	}

	#endregion

	protected virtual void SwitchState(CharacterState state) {
		this._state = state;
	}



	public override void _Ready()
	{
		base._Ready();
		faceAtPoint = Position + Vector2.Up* 3;
		this.prevPosition = GlobalPosition;
		loadHurtboxes();
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

	Vector2 prevPosition;
	public override void _PhysicsProcess(double delta)
	{
		
		runPhysics(delta);
		runPostPhysics(delta);
	}

	protected virtual void runPhysics(double delta) {
		///GD.Print(moveDir);
		KinematicCollision2D collision = MoveAndCollide( moveDir * (float)speed * (float)delta);
		LookAt(faceAtPoint);
		this.Rotation = this.Rotation + Mathf.DegToRad(90);

		/// handle collisions
		;

		if (collision != null) {
			
			onCollide(collision);
		}
	}

	protected virtual void onCollide(KinematicCollision2D collision2D) {
		/// TODO IMPLEMENT SLIDE
	}

	protected virtual void runPostPhysics(double delta) {
		this.decayPoise(delta);
		this._velocity = (GlobalPosition - prevPosition)/(float)delta;
		this.prevPosition = GlobalPosition;
	}

	#region IPoise
	public float getPoise()
	{
		return poise;
	}

	public virtual void setPoise(float poise)
	{
		this.poise = 0;
		this.addPoise(poise);
	}

	public virtual void decayPoise(double delta)
	{
		if (this.poise > 0) {
			this.poise -= (float)delta * this.POISE_DECAYRATE;
			this.poise = Mathf.Clamp(this.poise, 0,MAX_POISE);
		}	
	}
	
	public virtual void addPoise(float poise) {
		this.poise += poise;
		this.poise = Mathf.Clamp(this.poise, 0,MAX_POISE);

		GD.Print(this.poise);
	}
	#endregion

	#region IHurtbox

	protected void loadHurtboxes() {
		foreach (Node hurtbox in this.GetChildren()) {
			if (hurtbox is IHurtbox) {
				hurtboxes.Add((IHurtbox)hurtbox);
			}
		}

		enable();
	}
	public Node getHurtboxOwner()
	{
		return this;
	}

	public void enable()
	{	
		foreach (IHurtbox hurtbox in this.hurtboxes) {
			hurtbox.enable();
		}
	}

	public void disable()
	{
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
