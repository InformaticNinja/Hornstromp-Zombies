using Godot;
using System;

public class WhiteWeapon : Weapon
{
    public Area2D Weapon;
    public AnimationPlayer AttackAnimation;
    public override void _Ready()
    {

        base._Ready();
        
        Weapon = GetNode("Weapon") as Area2D;
        AttackAnimation = GetNode("AttackAnimation") as AnimationPlayer;
        WeaponSprite = GetNode<AnimatedSprite>("Weapon/AnimatedSprite");

    }

    public override void _OnCoolDownTimeout(){

        if(isAttacking){
            
            Attack(attackDirection);

        }

    }


    public virtual void _OnAreaEntered(Area2D area){

        if(area.IsInGroup("EnemiesHitbox")){

            (area.Owner as Enemie).Damage(damage, Player);

        }


    }

}
