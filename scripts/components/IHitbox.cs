using Godot;
using System;

public interface IHitbox
{
    
    /// <summary>
    /// When a collision occurs send a signal
    /// </summary>
    /// <param name="hurtbox"></param>
    [Signal]
	public delegate void HitboxStruckEventHandler(Node node);
	/**
	Returns the character associated with this
	*/
	public Node getOwner();
    
	public void enable();

	public void disable();

	public bool isEnabled();
}
