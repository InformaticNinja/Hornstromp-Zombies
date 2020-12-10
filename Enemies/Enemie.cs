using Godot;
using System;

public class Enemie : KinematicBody2D
{
    
    AnimationPlayer StatesAnimation;

    public override void _Ready()
    {
        
        StatesAnimation = GetNode("StatesAnimation") as AnimationPlayer;

    }


    public virtual void Damage(){

        StatesAnimation.Play("Damage");

    }

}
