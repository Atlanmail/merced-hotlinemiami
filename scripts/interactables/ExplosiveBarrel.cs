using Godot;
using System;
using System.Collections.Generic;
using System.Data.Common;

public partial class ExplosiveBarrel : Interactable, iPoise, IHurtbox, IHitboxOwner
{

	bool isExploding = false;
	float poise = 0;

	[Export]
	float MAX_POISE = 50; // how much poise it can take before exploding

	float EXPLOSION_RADIUS = 10; // how far the explosion will reach, scale of the barrel
	[Export]
	float TIME_EXPLODE = 2f; // how long before the barrel explodes
	float DAMAGE = 100f;
	float timer;
	Hitbox _hitbox;
	AnimatedSprite2D _sprite;
	public override void _Ready()
	{
		base._Ready();
		_hitbox = (Hitbox)NodeExtensions.FindFirstChildOfName(this, "Hitbox");

		if (_hitbox == null) {
			GD.PushError("Hitbox not found in ExplosiveBarrel!");
			SwitchState(InteractableState.Disabled);
		}
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (_sprite == null) {
			GD.PushError("AnimatedSprite2D not found in ExplosiveBarrel!");
			SwitchState(InteractableState.Disabled);
		}

		_hitbox.disable();
		_hitbox.Scale = new Vector2(EXPLOSION_RADIUS, EXPLOSION_RADIUS);

		SwitchState(InteractableState.Idle);
	}

	protected override void SwitchState(InteractableState newState)
	{
		base.SwitchState(newState);
		if (this.state == InteractableState.Idle) {
			isExploding = false;
			timer = TIME_EXPLODE;
		}

		if (this.state == InteractableState.Disabled) {
			this._hitbox.disable();

		}

	}

	public override void Grab(Node2D grabPoint)
	{
		base.Grab(grabPoint);
		isExploding = true;
	}

	protected override void onCollide(Node node)
	{
		this.Velocity = Vector2.Zero;

		explode();
	}

	protected virtual void explode() {
		_sprite.Play("RESET");
		_sprite.Play("explode");
		Godot.Collections.Array<Node2D> bodies = _hitbox.GetOverlappingBodies();
		Godot.Collections.Array<Area2D> areas = _hitbox.GetOverlappingAreas();
		foreach (Node2D body in bodies)
		{
			if (body is iPoise)
			{
				((iPoise)body).addPoise(DAMAGE);
			}
		}

		foreach (Area2D area in areas)
		{
			if (area is IHurtbox) 
			{
				Node owner = ((IHurtbox)area).getHurtboxOwner();
				if (owner is TileMapLayer) {
					TileMapLayer mapLayer= (TileMapLayer)owner;
					Vector2 collisionPos = area.GetPosition() ;
					Vector2I tileCoords = mapLayer.LocalToMap(collisionPos);
					mapLayer.EraseCell(tileCoords); // 0 is layer index
				}
			}
		}

		SwitchState(InteractableState.Disabled);
	}

	protected override void postPhysics(double delta)
	{
		base.postPhysics(delta);
		if (isExploding) {
			timer -= (float)delta;
			if (timer <= 0f) {
				explode();
			}
		}
	}

	#region iPoise
	public void addPoise(float poise)
	{
		if (state == InteractableState.Disabled) {
			return;
		}
		this.poise += poise;
		this.poise = Mathf.Clamp(this.poise, 0, MAX_POISE);
		_sprite.Play("onDamage");
		if (this.poise >= MAX_POISE) {
			isExploding = true;
		}
	}

	public void decayPoise(double delta)
	{
		return;
	}

	public float getPoise()
	{
		return this.poise;
	}

	public void setPoise(float poise)
	{
		this.poise = poise;
		this.addPoise(poise);
	}
	#endregion
}
