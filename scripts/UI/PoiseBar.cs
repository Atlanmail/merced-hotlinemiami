using Godot;
using System;

public partial class PoiseBar : ProgressBar
{
	
	[Export]
	Node defaultPoise;

	iPoise _iPoise;

	public void setTarget(iPoise newPoise) {
		_iPoise = newPoise; 
	}

	public override void _Ready()
	{
		base._Ready();
		if (defaultPoise is iPoise) {
			_iPoise = (iPoise)defaultPoise;
		}
	}

	public override void _Process(double delta) {
		if (_iPoise==null) {
			return;
		}
		Value = _iPoise.getPoise();
		
		if (Value > 0.75f) {
			
		}
	}
}
