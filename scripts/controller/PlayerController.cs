using Godot;
using System;

public partial class PlayerController : Node
{
	[Export]
	private int speed = 300;

	private Character _character;
	private Vector2 GetInput()
	{
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		return inputDir;
		
	}
	public override void _Ready()
	{
		
		_character = GetParent<Character>();
		GD.Print(_character);
	}

	
	public int getSpeed()
	{
		return _character.getSpeed();
	}

	public void setSpeed(int speed) {
		this.speed = speed;
		_character.setSpeed(speed);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 inputDir = GetInput();
		
		_character.move(inputDir.X + _character.Position.X, _character.Position.Y + inputDir.Y);
	}

}
