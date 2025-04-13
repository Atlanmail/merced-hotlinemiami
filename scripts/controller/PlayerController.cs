using Godot;
using System;

public partial class PlayerController : Node
{


	private static MainCharacter _character;

	public static MainCharacter MainCharacter { get { return _character; } }
	private Vector2 GetInput()
	{
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		return inputDir;
		
	}
	public override void _Ready()
	{
		
		_character = GetParent<MainCharacter>();
		GD.Print(_character);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 inputDir = GetInput();
		_character.move(inputDir.X + _character.Position.X, _character.Position.Y + inputDir.Y);
		
		Vector2 mousePos = _character.GetGlobalMousePosition();
		_character.face(mousePos);
	}

	public override void _UnhandledInput(InputEvent @event)
	{

		if (@event.IsActionPressed("left_action"))
		{
			_character.left_action();
		}
		else if (@event.IsActionPressed("right_action")) {
			_character.right_action();
		}
	}

}
