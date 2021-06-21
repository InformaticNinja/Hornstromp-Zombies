using Godot;
using System;

public class Character : KinematicBody2D
{
    public Godot.Collections.Array<Weapon> weapons = new Godot.Collections.Array<Weapon>(new Weapon[3]);
    protected Node2D ammoNode;
    protected Timer WeaponTimer;
    protected AnimationPlayer StatesAnimation;
    public Weapon currentWeapon;

    public Vector2 velocity = new Vector2();
    public float maxHp = 100;
    public float hp = 100;
    public float speed = 500f;
    public int coins = 0;

    public int COINS {get => this.coins; set => this.coins = value;}
    
    public override void _Ready(){
        
        WeaponTimer = GetNode("WeaponTimer") as Timer;

        StatesAnimation = GetNode<AnimationPlayer>("StatesAnimation");        
    }


}
