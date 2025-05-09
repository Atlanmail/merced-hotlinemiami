using Godot;
using System;
using System.Diagnostics;

public partial class CrowbarEnemy : Character, iGrabbable {


	Hitbox weaponHitbox;
	AnimationPlayer _animationPlayer;

	Node2D grabber; /// who controls this transform currently;
	[Export]
	public float THROW_DAMAGE = 80f; /// how much damage the object does when thrown
	[Export]
	public float CROWBAR_DAMAGE = 25f;
	[Export]
	public float THROW_VELOCITY = 100f; // pixels per second

	[Export]
	public float UNSTEADY_THRESHOLD = 90f; /// how much poise before the character becomes unsteady

	public int UNSTEADY_SPEED = 50;

	public override void _Ready()
	{
		base._Ready();

		weaponHitbox = (Hitbox)NodeExtensions.FindFirstChildOfName(this, "CrowbarHitbox");

		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_animationPlayer.AnimationFinished += this.onAnimationEnd;
		weaponHitbox.HitboxStruck += this.onWeaponHit;

	}

	protected override void SwitchState(CharacterState state) {
		if (_state == CharacterState.Dead) {
			return;
		}
		switch(state) {
			case CharacterState.Idle:
				if (this.getPoise() > UNSTEADY_THRESHOLD) {
					SwitchState(CharacterState.Unsteady);
				}
				else {
					this.setSpeed(DEFAULT_SPEED);
					_state = CharacterState.Idle;
				}
				break;
			case CharacterState.Unsteady:
				_state = CharacterState.Unsteady;
				this.setSpeed(UNSTEADY_SPEED);
				break;
			case CharacterState.AttackingV1:
				if (_state == CharacterState.Struck || _state == CharacterState.Grabbed) {
					return;
				}
				_state = CharacterState.AttackingV1;
				break;
			case CharacterState.Struck:
				_state = CharacterState.Struck;
				break;
			case CharacterState.Thrown:
				_state = CharacterState.Thrown;
				
				this.SetCollisionLayerValue(2, true);
				this.SetCollisionMaskValue(1, true);
				this.SetCollisionMaskValue(2, true);
				this.SetCollisionMaskValue(3, true);
				this.SetCollisionMaskValue(4, true);
				break;
			case CharacterState.Grabbed:
				_state = CharacterState.Grabbed;
				this.CollisionLayer = 0;
				this.CollisionMask = 0;
				break;
			case CharacterState.Dead:
				_state = CharacterState.Dead;
				break;
		}
		
	}

	protected override void onCollide(KinematicCollision2D collision)
	{
		if (_state == CharacterState.Thrown) {
			GodotObject body2D = collision.GetCollider();
			if (body2D is IHurtbox) {
				if (body2D is TileMapLayer) {
					TileMapLayer mapLayer= (TileMapLayer)body2D;
					Vector2 collisionPos = collision.GetPosition() + velocity * 5f; /// add a slight offset to gurantee position
					Vector2I tileCoords = mapLayer.LocalToMap(collisionPos);
					mapLayer.EraseCell(tileCoords); // 0 is layer index
				}

				else {
					Node owner = (body2D as IHurtbox).getHurtboxOwner();
					if (owner is iPoise) {
						(owner as iPoise).addPoise(THROW_DAMAGE);
					}   
					this.SwitchState(CharacterState.Dead);
				}
			}
		}
	}

	protected override void runPhysics(double delta) {
		///GD.Print(moveDir);
		if (_state == CharacterState.Idle || _state == CharacterState.Unsteady) {
			walk(delta);
		}
		else if (_state == CharacterState.Grabbed) {
			GD.Print(grabber.Name);
			Position = grabber.GlobalPosition;
		}

		else if (_state == CharacterState.Thrown) {
			var collision = MoveAndCollide(velocity * (float)delta);
			if (collision != null)
			{
				onCollide(collision);
			}
		}

		
	}

	public void left_action()
	{
		_animationPlayer.ClearQueue();
		_animationPlayer.Play("crowbar_swing"); 
		this.SwitchState(CharacterState.AttackingV1);
		weaponHitbox.enable();

	}
	private void onAnimationEnd(StringName myString) {

		if (myString == "crowbar_swing") {
			weaponHitbox.disable();
		}
	}

	private void onWeaponHit(Node node) {
		if (_state != CharacterState.AttackingV1 || this == node) {
			return;
		}
		if (node is not IHurtbox && node is not IHitbox) {
			onAnimationEnd("crowbar_swing");
			return;
		}

		Node owner = (node as IHurtbox).getHurtboxOwner();
		if (owner == this) {
			return;
		}
		if (owner == null) {
			onAnimationEnd("crowbar_swing");
			return;
		}


		if (owner is iPoise) {
			(owner as iPoise).addPoise(CROWBAR_DAMAGE);
		}

		onAnimationEnd("crowbar_swing");
		return;

	}
	#region Grabbable
	public bool IGrabbable()
	{
		GD.Print("Am I grabbable: " + (_state == CharacterState.Unsteady).ToString());
		if (_state == CharacterState.Unsteady) {
			return true;
		}
		else {
			return false;
		}
	}

	public void Grab(Node2D grabPoint)
	{
		if (IGrabbable() == false) {
			GD.PushError("Cannot grab this object");
			return;
		}
		GD.Print("I am grabbed!");
		grabber = grabPoint;

		GD.Print(grabber.Name);
		SwitchState(CharacterState.Grabbed);
		
	}

	public void Release()
	{
		throw new NotImplementedException();
	}

	public void Throw(Vector2 target)
	{
		velocity = velocity*0.5f + (target - GlobalPosition).Normalized() * THROW_VELOCITY;

		this.GlobalPosition = GlobalPosition + (target - GlobalPosition).Normalized() * 35f; /// slight offset to prevent self collisions
		this.SwitchState(CharacterState.Thrown);
	}

	public ThrowData GetThrowData()
	{
		/// TODO, add mass to the object
		return new ThrowData(1f, THROW_VELOCITY);
	}

	#endregion


	#region IPoise

	public override void decayPoise(double delta)
	{
		base.decayPoise(delta);
		if (poise < UNSTEADY_THRESHOLD) {
			this.SwitchState(CharacterState.Idle);
		}
	}
	
	public override void addPoise(float poise) {
		base.addPoise(poise);
		if (poise > UNSTEADY_THRESHOLD) {
			this.SwitchState(CharacterState.Unsteady);
		}
	}
	#endregion


}
