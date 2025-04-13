using Godot;
using System;

public partial class Interactable : CharacterBody2D, iGrabbable, IHurtbox
{
	[Export]
	public float MASS; 
	[Export]
	public float THROW_VELOCITY = 200f; // pixels per second 

	private Node2D _grabber; /// who controls this transform currently
	private InteractableState _state;

	private Vector2 velocity;

	protected enum InteractableState {
		Disabled,
		Idle,
		Grabbed,
		Thrown
	}

	private void SwitchState(InteractableState newState) {
		if (_state == newState) {
			GD.PushWarning("State is already " + newState);
			return;
		}
		_state = newState;

		this.CollisionLayer = 0;
		this.CollisionMask = 0;
		if (_state == InteractableState.Idle) {
			this._grabber = null;
			this.SetCollisionLayerValue(4, true);
			this.SetCollisionLayerValue(1, true);

			this.SetCollisionMaskValue(1, true);
			this.SetCollisionMaskValue(2, true);
			this.SetCollisionMaskValue(3, true);
			this.SetCollisionMaskValue(4, true);
		}

		else if (_state == InteractableState.Grabbed) {
			this.SetCollisionMaskValue(1, false);
			this.SetCollisionMaskValue(2, false);
			this.SetCollisionMaskValue(3, false);
			this.SetCollisionMaskValue(4, false);
		}
		else if (_state == InteractableState.Thrown) {
			this.SetCollisionLayerValue(4, true);
			this.SetCollisionMaskValue(1, true);
			this.SetCollisionMaskValue(2, true);
			this.SetCollisionMaskValue(3, true);
			this.SetCollisionMaskValue(4, true);
		}

		
	}
	public override void _Ready()
	{
		base._Ready();
		SwitchState(InteractableState.Idle);
		
	}
	public ThrowData GetThrowData()
	{
		return new ThrowData(MASS, THROW_VELOCITY);
	}

	public void Grab(Node2D grabPoint)
	{
		if (IGrabbable() == false) {
			GD.PushError("Cannot grab this object");
			return;
		}

		SwitchState(InteractableState.Grabbed);
		_grabber = grabPoint;

	}

	public bool IGrabbable()
	{
		if (_state != InteractableState.Idle) {
			return false;
		}
		return true;
	}

	/**
	Behaves similarly to a throw without being picked up
	*/
	public void shove(int x, int y, float force)
	{
		throw new NotImplementedException();
	}

	private Vector2 prevPosition = Vector2.Zero;
	public override void _PhysicsProcess(double delta)
	{

		if (_state == InteractableState.Grabbed) { /// TODO, ADD MORE NATURAL MOTION
			
			Position = _grabber.GlobalPosition;
			velocity = (GlobalPosition - prevPosition);
			velocity = velocity/ (float)delta;	
			GD.Print(velocity);
		}

		else if (_state == InteractableState.Thrown) {
			var collision = MoveAndCollide(velocity * (float)delta);
			if (collision != null)
			{
				onCollide((Node)collision.GetCollider());
			}
		}
		prevPosition = GlobalPosition;
	}

	protected virtual void onCollide(Node node) {
		this.velocity = Vector2.Zero;
		SwitchState(InteractableState.Idle);


	}

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
		velocity = velocity*0.5f + (target - GlobalPosition).Normalized() * THROW_VELOCITY;

		this.GlobalPosition = GlobalPosition + (target - GlobalPosition).Normalized() * 50f; /// slight wwwoffset to prevent self collisions
		this.SwitchState(InteractableState.Thrown);
		
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
