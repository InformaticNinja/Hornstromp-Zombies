using Godot;
using System;

public class Weapon : Node2D
{

    [Export] int damage;
    [Export] protected float timeCoolDown;
    protected Timer CoolDown;
    protected Vector2 attackDirection;
    protected bool isAttacking = false;
    public String weaponInfo;

    protected Player Player;
    

    public override void _Ready()
    {
        CoolDown = GetNode("CoolDown") as Timer;

        SetPhysicsProcess(false);

        Visible = false;
    }

    public void Start(Player player){

        Player = player;

    }

    public virtual void Attack(Vector2 direction){
    }

    public virtual void JoystickPressed(bool pressed){

        isAttacking = pressed;

        if(!pressed){

            attackDirection = Vector2.Zero;

        }
    }

    public virtual void Aim(Vector2 direction){

    }

    public virtual void Enter(){

        Visible = true;

    }

    public virtual void Exit(){

        Visible = false;

    }

    public virtual void _OnCoolDownTimeout(){ 
    }

}
