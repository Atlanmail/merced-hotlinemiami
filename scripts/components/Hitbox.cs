using Godot;
using System;

/**
Hitboxes that attacks use to see if things are hurt
*/
public partial class Hitbox : Area2D, IHitbox
{
	/**
	It is the subscribers responsibility to avoid debounces
	*/
	[Signal]
	public delegate void HitboxStruckEventHandler(Node node);
	IHitboxOwner _owner;
	bool enabled = false;

	public override void _Ready()
	{
		base._Ready();

		Node current = this	;
		while (current != null)
		{
			if (current is IHitboxOwner)
			{	
				GD.Print("Found hitboxOwner " + current.Name);
				_owner = (IHitboxOwner)current;
				break;
			}    
			current = current.GetParent();
		}
		if (_owner == null) {
			GD.PushError("Parent of hitbox is not an IHitboxOwner");
			return;
		}

		this.CollisionLayer = 0;
		this.CollisionMask = 0;

		this.SetCollisionLayerValue(3, true);
		this.SetCollisionMaskValue(1, true);
		this.SetCollisionMaskValue(3, true);

		

		BodyEntered += OnBodyEntered;
		AreaEntered += OnAreaEntered;

	}

	/**
	*/
	private void OnBodyEntered(Node body)
	{
		EmitSignal(SignalName.HitboxStruck, body);
	}

	private void OnAreaEntered(Area2D area)
	{
		EmitSignal(SignalName.HitboxStruck, area);
	}


	/**
	Returns the character associated with this
	*/
	public IHitboxOwner getOwner() {
		return _owner;
	}   

	public void enable() {
		enabled = true;

		foreach (Area2D area in GetOverlappingAreas())
		{
			OnAreaEntered(area);
		}

		// Get all overlapping bodies
		foreach (Node2D body in GetOverlappingBodies())
		{
			if (body is Node2D node2D)
			{
				OnBodyEntered(node2D);
			}
		}

	}

	public void disable() {
		enabled = false;
	}

	public bool isEnabled() {
		return enabled;
	}
}
