using Godot;
using System;

public partial class Root : Node2D
{
	[Export]
	private Node mainCharacter;
	
	private Character _mainCharacter;
	public override void _Ready()
	{
		base._Ready();
	
		if (mainCharacter is Character) {
			_mainCharacter = (Character)mainCharacter;
		}
	}

	
	public Character MainCharacter() {
		return _mainCharacter;
	}
}
