using Godot;
using System;

public partial class Character : CharacterBody2D, iMove
{
	[Export]
	private int speed = 300;
	
	[Export]
	private float poise = 0; 
	[Export]
	private float poiseDecayRate = 0; /// how much the health decays per second
	
	private Vector2 moveDir = new Vector2(0,0);
	CharacterBody2D characterBody2D;

	public int getSpeed()
	{
		return speed;
	}

	public void setSpeed(int speed) {
		this.speed = speed;
	}



	/**

	*/

	public void move(float x, float y)
	{
		moveDir = new Vector2(x,y) - Position;
		moveDir = moveDir.Normalized();
	}

	public void shove(int x, int y, float force)
	{
		throw new NotImplementedException();
	}

	public override void _PhysicsProcess(double delta)
	{
		///GD.Print(moveDir);
		KinematicCollision2D collision = MoveAndCollide( moveDir * (float)speed * (float)delta);

		/// handle collisions
		;

		if (collision != null) {
			GodotObject body2D = collision.GetCollider();
			if (body2D is TileMapLayer) {
				TileMapLayer mapLayer= (TileMapLayer)body2D;
				Vector2 collisionPos = collision.GetPosition() + moveDir * 5f; /// add a slight offset to gurantee position
				Vector2I tileCoords = mapLayer.LocalToMap(collisionPos);

				mapLayer.EraseCell(tileCoords); // 0 is layer index
			}
		}

		
	   

	}





}
