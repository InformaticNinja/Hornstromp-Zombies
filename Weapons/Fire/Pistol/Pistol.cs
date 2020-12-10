using Godot;
using System;

public class Pistol : FireArm
{

    public override void JoystickPressed(bool pressed)
    {

        isAttacking = pressed;

        Crosshair.Visible = false;

        if(!pressed){

            Attack(attackDirection);

        }
    }
    public override void Attack(Vector2 direction){

        Shot.CastTo = direction * scope;

        SetPhysicsProcess(true);
        
    }

}
