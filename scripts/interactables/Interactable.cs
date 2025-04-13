using Godot;
using System;
using System.Net;

public partial class Interactable : CharacterBody2D, iGrabbable, IHurtbox
{
	#region Variables
	[Export]
	public float MASS; 
	[Export]
	public float THROW_VELOCITY = 200f; // pixels per second 

	private Node2D _grabber; /// who controls this transform currently
	private InteractableState _state;

	protected InteractableState state {
		get { return _state; }
	}
	private Vector2 _velocity;
	protected Vector2 velocity {
		get { return _velocity; }
	}
	#endregion

	#region State Machine
	protected enum InteractableState {
		Disabled,
		Idle,
		Grabbed,
		Thrown
	}

	protected virtual void SwitchState(InteractableState newState) {
		if (_state == newState) {
			GD.PushWarning("State is already " + newState);
			return;
		}
		_state = newState;

		if (_state == InteractableState.Idle) {
			this.CollisionLayer = 0;
			this.CollisionMask = 0;
			this._grabber = null;
			this.SetCollisionLayerValue(4, true);
			this.SetCollisionLayerValue(1, true);

			this.SetCollisionMaskValue(1, true);
			this.SetCollisionMaskValue(2, true);
			this.SetCollisionMaskValue(3, true);
			this.SetCollisionMaskValue(4, true);
		}

		else if (_state == InteractableState.Grabbed) {
			this.CollisionLayer = 0;
			this.CollisionMask = 0;
			this.SetCollisionMaskValue(1, false);
			this.SetCollisionMaskValue(2, false);
			this.SetCollisionMaskValue(3, false);
			this.SetCollisionMaskValue(4, false);
		}
		else if (_state == InteractableState.Thrown) {
			this.CollisionLayer = 0;
			this.CollisionMask = 0;
			this.SetCollisionLayerValue(4, true);
			this.SetCollisionMaskValue(1, true);
			this.SetCollisionMaskValue(2, true);
			this.SetCollisionMaskValue(3, true);
			this.SetCollisionMaskValue(4, true);
		}
		else if (_state == InteractableState.Disabled) {
			
		}

		
	}
	#endregion
	public override void _Ready()
	{
		base._Ready();
		SwitchState(InteractableState.Idle);
		
	}

	#region IGrabbable
	public ThrowData GetThrowData()
	{
		return new ThrowData(MASS, THROW_VELOCITY);
	}

	public virtual void Grab(Node2D grabPoint)
	{
		if (IGrabbable() == false) {
			GD.PushError("Cannot grab this object");
			return;
		}

		SwitchState(InteractableState.Grabbed);
		_grabber = grabPoint;

	}

	public virtual bool IGrabbable()
	{
		if (_state != InteractableState.Idle) {
			return false;
		}
		return true;
	}

	/**
	 * Called when the object is released, meaning it is dropped or something along the lines
	 */
	public void Release()
	{
		throw new NotImplementedException();
	}

/**
	 * Throws the object in the direction of the target
	 * @param target the global position to throw towards
	 */
	public void Throw(Vector2 target)
	{
		_velocity = _velocity*0.5f + (target - GlobalPosition).Normalized() * THROW_VELOCITY;

		this.GlobalPosition = GlobalPosition + (target - GlobalPosition).Normalized() * 35f; /// slight offset to prevent self collisions
		this.SwitchState(InteractableState.Thrown);
		
	}
	#endregion
	/**
	Behaves similarly to a throw without being picked up
	*/
	public virtual void shove(int x, int y, float force)
	{
		throw new NotImplementedException();
	}

	private Vector2 prevPosition = Vector2.Zero;
	public override void _PhysicsProcess(double delta)
	{
		if (InteractableState.Disabled == _state) {
			return;
		}
		runPhysics(delta);
		postPhysics(delta);
	}

	/**
	 * Called for the physics process
	 */
	protected virtual void runPhysics(double delta) {
		if (_state == InteractableState.Grabbed) { /// TODO, ADD MORE NATURAL MOTION
			
			Position = _grabber.GlobalPosition;
			///GD.Print(velocity);
		}

		else if (_state == InteractableState.Thrown) {
			var collision = MoveAndCollide(_velocity * (float)delta);
			if (collision != null)
			{
				onCollide((Node)collision.GetCollider());
			}
		}
	}
	/**
	 * Called after the physics process ideally
	 */
	protected virtual void postPhysics(double delta) {
		
		_velocity = (GlobalPosition - prevPosition);
		_velocity = _velocity/ (float)delta;	
		prevPosition = GlobalPosition;
	}

	protected virtual void onCollide(Node node) {
		this._velocity = Vector2.Zero;
		SwitchState(InteractableState.Idle);
	}

	

	public Node getHurtboxOwner()
	{
		return this;
	}

	public void enable()
	{
		SwitchState(InteractableState.Idle);
	}

	public void disable()
	{
		SwitchState(InteractableState.Disabled);
	}

	public bool isEnabled()
	{
		return _state != InteractableState.Disabled;
	}
}
