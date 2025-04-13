using Godot;
using System;

public partial class CrowbarEnemyController : Node
{
	[Export]
	public bool Enabled = true;
	private static float AGGRO_RADIUS = 200f;
	private static float ATTACK_RADIUS = 30f;


	private CrowbarEnemy _character;

	private MainCharacter mainCharacter;
	
	public override void _Ready()
	{
		
		_character = GetParent<CrowbarEnemy>();
		
		if (_character == null) {
			GD.PushError("Character controller has no character");
		}


	}

	public override void _PhysicsProcess(double delta)
	{
		if (!Enabled) {
			return;
		}
		Vector2 playerGlobalPos = PlayerController.MainCharacter.Position;
		///GD.Print(playerGlobalPos - _character.Position);
		if ((playerGlobalPos - _character.Position).Length() < AGGRO_RADIUS) {
			_character.move(playerGlobalPos.X, playerGlobalPos.Y);
			_character.face(playerGlobalPos);
		}

		if ((playerGlobalPos - _character.Position).Length() < ATTACK_RADIUS) {
			_character.left_action();
		}

		
	}

}
