using Godot;
using System;

public class Knife : WhiteWeapon
{
    public override void _Ready()
    {
        base._Ready();

        Weapon.Visible = false;
    }


    public override void Aim(Vector2 direction){

        base.Aim(direction);

        attackDirection = direction;

        if(CoolDown.IsStopped() && direction != Vector2.Zero)

            Attack(attackDirection);

    }

    public override void Attack(Vector2 direction)
    {

        if(automatic){

            AutomaticTarget(direction);

        }

        Rotation = direction.Angle();

        Weapon.Visible = true;

        AttackAnimation.Play("Attack");

        Weapon.Monitoring = true;

        CoolDown.Start(timeCoolDown);

        base.Attack(direction);
    }

    public void _OnAttackAnimationFinished(String animName){

        Weapon.Monitoring = false;

        Weapon.Visible = false;

    }


    public void _OnWeaponBodyEntered(Node body){

        if(body.IsInGroup("Enemies")){

            ((Enemie)body).Damage(damage, Player);

        }

    }


}
