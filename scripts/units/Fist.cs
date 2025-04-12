using Godot;
using System;

public partial class Fist : Area2D
{
	#region Variables
	[Export]
	float POSITION_THRESHOLD = 10f; /// determines if the guy keeps on moving

	/// <summary>
	/// Desired offset
	/// </summary>

	[Export]
	Vector2 desiredOffset = new Vector2(0,0);
	[Export]
	int speed = 20;

	[Signal]
	public delegate void CollisionEventHandler(Node other);

	Character _character;

	public void setSpeed(int speed) {
		this.speed = speed;
	}


	#endregion 
	public override void _Ready()
	{
		
		_character = GetParent<Character>();
		TopLevel = true;
		this.BodyEntered += OnBodyEntered;

	}

	
	#region Physics
	private void OnBodyEntered(Node body)
	{
		if (body == _character) {
			return;
		}
		GD.Print("Collided with " + body);
		EmitSignal(nameof(CollisionEventHandler), body);
	}


	public override void _PhysicsProcess(double delta)
	{
		///GD.Print(moveDir);
		/// 
		Vector2 moveDir = _character.GlobalPosition + desiredOffset.Rotated(_character.Rotation) - this.GlobalPosition;

		if (moveDir.Length() > POSITION_THRESHOLD) {
			GlobalPosition = GlobalPosition + moveDir.Normalized() * (float)speed * (float)delta;
		}
		

		/// handle collisions
		;

		/**if (collision != null) {
			GodotObject body2D = collision.GetCollider();
			if (body2D is TileMapLayer) {
				TileMapLayer mapLayer= (TileMapLayer)body2D;
				Vector2 collisionPos = collision.GetPosition() + moveDir * 5f; /// add a slight offset to gurantee position
				Vector2I tileCoords = mapLayer.LocalToMap(collisionPos);

				mapLayer.EraseCell(tileCoords); // 0 is layer index
			}
		}**/

		
	   

	}
	#endregion
}
