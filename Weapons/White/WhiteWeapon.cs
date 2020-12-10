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

    }

    public override void _OnCoolDownTimeout(){

        if(isAttacking){
            
            Attack(attackDirection);

        }

    }




}
