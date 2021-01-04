using Godot;
using System;

public class EnemieState : State
{
    
    Enemie Enemie;
    public override void _Ready()
    {

        Enemie = Owner as Enemie;
        
    }

    public override void Update(float delta){

        Enemie.MoveAndSlide(Enemie.velocity * Enemie.speed);

    }

}
