using Godot;
using System;


public struct ThrowData
{

    public ThrowData(float mass, float throwVelocity) {
        this.mass = mass;
        this.throwVelocity = throwVelocity;
    }
    /**
        Mass allows for combat calculations
    */
    public float mass; 
    /**
    Determines the release velocity in meters per second
    */
    public float throwVelocity;
}
public interface iGrabbable
{
    /**
        cues the target to escape for whateber reason
    */
    public delegate void EscapeEventHandler();

    public bool IGrabbable();

    /**
        Sets the objects state to being grabbed while rooting them to the anchor
    */
    public void Grab(Node2D grabPoint);

    /**
        Tells the object its released    
    */
    public void Release();

    /**
    Puts the object in a throw state 
    */
    public void Throw();
    /**
        Returns the throwData for calculations
    */
    public ThrowData GetThrowData();

}
