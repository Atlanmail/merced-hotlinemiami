using Godot;
using System;

public interface iMove
{
	/**
	Speed is in pixel /s
	*/
	public void setSpeed(int speed);

	/**
	Attempts to get the object to face a global position
	
	*/
	public void face(Vector2 globalPosition);
	public int getSpeed();

	/**
	Shoves the entity in the specified direction 
	Returns 0 on success
	*/
	public void shove(int x, int y, float force);

	/**
	Tells the entity to move towards a specified position.
	Returns 0 on
	*/
	public void move(float x, float y);

}
