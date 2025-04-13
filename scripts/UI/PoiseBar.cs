using Godot;
using System;

public partial class PoiseBar : ProgressBar
{
	
	[Export]
	Node defaultCharacterPoise;

	iPoise _iPoise;
	Vector2 offset = Vector2.Down*2;
	bool isOnEntity = false;

	public void setTarget(iPoise newPoise) {
		_iPoise = newPoise; 
	}

	public override async void _Ready()
	{
		base._Ready();

		if (defaultCharacterPoise is iPoise) {
			_iPoise = (iPoise)defaultCharacterPoise;
		}
		else if (GetParent() is iPoise) {
			this.isOnEntity = true;
			this.TopLevel = true;
			_iPoise = (iPoise)GetParent();
		}
		else {
			 GD.PushError("_iPoise is not available");
		}
	   
		
	}

	public override void _Process(double delta) {
		if (_iPoise==null) {
			return;
		}
		if (this.isOnEntity == true) {
			this.GlobalPosition = ((Node2D)_iPoise).GlobalPosition + offset;
		}

		this.Value = _iPoise.getPoise();
		
		if (Value > 0.75f) {
			
		}
	}
}
