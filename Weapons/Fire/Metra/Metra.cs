using Godot;
using System;

public class Metra : FireArm
{
    public override void Aim(Vector2 direction)
    {

        base.Aim(direction);

        if(CoolDown.IsStopped()){

            Attack(attackDirection);

        }

    }

    public override void Attack(Vector2 direction){

        if(checkCharger()){

            Shot.CastTo = direction * scope;

        SetPhysicsProcess(true);

        CoolDown.Start(timeCoolDown);

        base.Attack(direction);

        }

    }

    public override void _OnCoolDownTimeout(){

        base._OnCoolDownTimeout();

        if(isAttacking){
            
            Attack(attackDirection);

        }

    }

}
