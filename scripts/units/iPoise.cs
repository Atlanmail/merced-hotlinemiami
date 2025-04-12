using Godot;
using System;

public interface iPoise
{

	/**
		Returns the current poise
	*/
	public float getPoise();
	/**
	Sets the poise value to a percentage
	*/
	public void setPoise(float poise);

	/**
	Call this every frame to decay
	*/
	public void decayPoise();

}
