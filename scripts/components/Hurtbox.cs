using Godot;
using System;

public partial class Hurtbox : Area2D, IHurtbox
{
	Character _character;
	bool enabled = false;

	public override void _Ready()
	{
		base._Ready();

		
		this.CollisionLayer = 0;
		this.CollisionMask = 0;
		this.SetCollisionLayerValue(3, true);
		this.SetCollisionMaskValue(3, true);

		_character = (Character)GetParent();

		if (_character == null) {
			GD.PushError("Parent of hitbox is not character");
		}
	}

	/**
	Returns the character associated with this
	*/
	public Node getHurtboxOwner() {
		return _character;
	}   

	public void enable() {
		enabled = true;
	}

	public void disable() {
		enabled = false;
	}

	public bool isEnabled() {
		return enabled;
	}
	
	
}
