using Godot;
using System;

public class Ammo : Area2D
{
    
    Vector2 velocity;
    public override void _Ready()
    {
        
    }

    public void start(Vector2 pos, Vector2 velocity){

        Position = pos;

        this.velocity = velocity;


    }

    public override void _PhysicsProcess(float delta)
    {

        Position += velocity * 5;

        base._PhysicsProcess(delta);
    }

}
