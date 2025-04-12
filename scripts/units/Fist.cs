using Godot;
using System;

public partial class Fist : Area2D
{
	const float POSITION_THRESHOLD = 2f; /// determines if the guy keeps on moving

	/// <summary>
	/// Desired offset
	/// </summary>

	[Export]
	Vector2 desiredOffset = new Vector2(0,0);

	[Export]
	int speed = 20;

	Character _character;

	public override void _Ready()
	{
		
		_character = GetParent<Character>();
		TopLevel = true;
		
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
}
