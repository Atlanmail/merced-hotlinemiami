using Godot;
using System;

public interface iPoise
{

    /**
        Returns the current poise
        Poise should be a float from 0 to 1.
    */
    public float getPoise();
    /**
    Sets the poise value to a percentage
    */
    public void setPoise(float poise);

    /**
    Adds the poise
    */

    public void addPoise(float poise);
    /**
    Call this every frame to decay
    */
    public void decayPoise();

}
