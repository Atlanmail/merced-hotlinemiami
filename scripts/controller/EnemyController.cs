using Godot;
using System;

public partial class EnemyController : Node
{
	private static float AGGRO_RADIUS = 200f;


	private Character _character;

	private MainCharacter mainCharacter;
	
	public override void _Ready()
	{
		
		_character = GetParent<Character>();
		
		if (_character == null) {
			GD.PushError("Character controller has no character");
		}


	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 playerGlobalPos = PlayerController.MainCharacter.Position;
		///GD.Print(playerGlobalPos - _character.Position);
		if ((playerGlobalPos - _character.Position).Length() < AGGRO_RADIUS) {
			_character.move(playerGlobalPos.X, playerGlobalPos.Y);
			_character.face(playerGlobalPos);
		}

		
	}

}
