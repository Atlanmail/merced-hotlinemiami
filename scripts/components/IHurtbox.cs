using Godot;
using System;

public interface IHurtbox
{
	/**
	Returns the character associated with this
	*/
	public Node getHurtboxOwner();

	public void enable();

	public void disable();

	public bool isEnabled();
	
	
}
