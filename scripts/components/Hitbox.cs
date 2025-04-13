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
	Character _character;
	bool enabled = false;

	public override void _Ready()
	{
		base._Ready();

		_character = NodeExtensions.FindFirstParent<Character>(this);

		if (_character == null) {
			GD.PushError("Parent of hitbox is not character");
			return;
		}

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
	public Node getOwner() {
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
