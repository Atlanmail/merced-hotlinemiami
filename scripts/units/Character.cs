using Godot;
using System;
using System.Collections.Generic;
using System.Security;

public partial class Character : CharacterBody2D, iMove, iPoise, IHitboxOwner
{

	protected enum CharacterState
	{
		Idle,
		Unsteady,
		Struck,
		Thrown,
		AttackingV1,
		AttackingV2,
		Charging,
		Grabbing,
		Grabbed, 
		Dead
	}


	#region Variables
	[Export]
	protected int DEFAULT_SPEED = 300;
	private int speed;
	
	protected CharacterState _state = CharacterState.Idle;

	[Export]
	protected float MAX_POISE = 100f;
	protected float poise = 0; 
	[Export]
	protected float POISE_DECAYRATE = 0; /// how much the health decays per second
	
	private Vector2 moveDir = new Vector2(0,0);
	private Vector2 faceAtPoint;
	
	List<IHurtbox> hurtboxes = new List<IHurtbox>();
	bool hurtboxEnabled = false;

	private Vector2 _velocity;
	protected Vector2 velocity {
		get { return _velocity; }
		set{
			_velocity = value;
		}
	}

	#endregion

	protected virtual void SwitchState(CharacterState state) {
		this._state = state;
	}



	public override void _Ready()
	{
		base._Ready();
		this.setSpeed(DEFAULT_SPEED);
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
		walk(delta);
	}

	protected virtual void walk(double delta) {
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

		enableHurtboxes();
	}
	public Node getHurtboxOwner()
	{
		return this;
	}

	public void enableHurtboxes()
	{	
		foreach (IHurtbox hurtbox in this.hurtboxes) {
			hurtbox.enable();
		}
	}

	public void disableHurtboxes()
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
