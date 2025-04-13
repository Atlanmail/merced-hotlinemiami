using Godot;
using System;


/**/

public partial class Interactable : CharacterBody2D, iGrabbable, IHurtbox
{
	[Export]
	public float mass; 
	[Export]
	public float throwVelocity;

	private Node2D _owner; /// who controls this transform currently
	private InteractableState _state = InteractableState.Idle;

	protected enum InteractableState {
		Disabled,
		Idle,
		Grabbed,
		Thrown
	}

	private void SwitchState(InteractableState newState) {
		if (_state == newState) {
			return;
		}
		_state = newState;

		this.CollisionLayer = 0;
		this.CollisionMask = 0;
		if (_state == InteractableState.Idle) {
			this.SetCollisionLayerValue(4, true);
			this.SetCollisionMaskValue(1, true);
			this.SetCollisionMaskValue(2, true);
			this.SetCollisionMaskValue(3, false);
			this.SetCollisionMaskValue(4, true);
		}

		else if (_state == InteractableState.Grabbed) {
		}

		
	}
	public override void _Ready()
	{
		base._Ready();
		SwitchState(InteractableState.Idle);
		
	}
	public ThrowData GetThrowData()
	{
		return new ThrowData(mass, throwVelocity);
	}

	public void Grab(Node2D grabPoint)
	{
		if (IGrabbable() == false) {
			GD.PushError("Cannot grab this object");
			return;
		}

		SwitchState(InteractableState.Grabbed);
		_owner = grabPoint;

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

	public override void _PhysicsProcess(double delta)
	{
		if (_state == InteractableState.Grabbed) { /// TODO, ADD MORE NATURAL MOTION
			
			Position = _owner.GlobalPosition;
		}
	}

	public void Release()
	{
		throw new NotImplementedException();
	}

	public void Throw()
	{
		throw new NotImplementedException();
	}

	public Node getHurtboxOwner()
	{
		return this;
	}

	public void enable()
	{
		_state = InteractableState.Idle;
	}

	public void disable()
	{
		_state = InteractableState.Disabled;
	}

	public bool isEnabled()
	{
		return (_state != InteractableState.Disabled);
	}
}
