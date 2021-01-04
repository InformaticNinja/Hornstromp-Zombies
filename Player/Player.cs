using Godot;
using System;

public class Player : KinematicBody2D
{
    public Godot.Collections.Array<Weapon> weapons = new Godot.Collections.Array<Weapon>(new Weapon[3]);
    
    Node2D ammoNode;
    PlayerFSM FSM;
    Timer WeaponTimer;
    AnimationPlayer StatesAnimation;
    public Weapon currentWeapon;
    PackedScene ammoLoad;

    public Vector2 velocity = new Vector2();
    public float maxHp = 100;
    public float hp = 100;
    public float speed = 500f;
    public int coins = 100;

    public int COINS {get => this.coins; set => this.coins = value;}

    [Signal] public delegate void WeaponInfo(String weaponInfo);
    [Signal] public delegate void ChangeCoins(int coins);
    [Signal] public delegate void ChangeHp(float hp);
    [Signal] public delegate void ChangeWeapons();

    public override void _Ready()
    {

        weapons[0] = (GD.Load("res://Weapons/White/Knife/Knife.tscn") as PackedScene).Instance() as WhiteWeapon;

        weapons[0].Start(this);

        GetNode("Weapons").AddChild(weapons[0]);

        FSM = GetNode("PlayerFSM") as PlayerFSM;

        ammoLoad = GD.Load("res://Weapons/Ammo/Ammo.tscn") as PackedScene;

        WeaponTimer = GetNode("WeaponTimer") as Timer;

        StatesAnimation = GetNode<AnimationPlayer>("StatesAnimation");
        
    }

    public void start(Node2D ammoNode){

        this.ammoNode = ammoNode;

        EmitSignal("ChangeCoins", coins);

        EmitSignal("ChangeHp", hp, maxHp);

    }

    public override void _PhysicsProcess(float delta)
    {

        if(FSM.active){

            FSM.currentState.Update(delta);

        }

        base._PhysicsProcess(delta);
    }

    public void JoystickInput(String joystick, Vector2 direction){

        if(joystick == "move"){

            FSM.currentState.JoystickMove(direction);

        }else if(joystick == "shoot"){

            currentWeapon.Aim(direction);

        }
    }

    public void JoystickPressed(String joystick, bool pressed, bool auto = false){

        if(joystick == "shoot"){

            currentWeapon.JoystickPressed(pressed, auto);

        }

        if(joystick == "move"){

            FSM.currentState.JoystickMove(Vector2.Zero);

        }

    }

    public void Shoot(Vector2 position, Vector2 velocity){

        if(WeaponTimer.IsStopped()){

            Ammo ammo = ammoLoad.Instance() as Ammo;

            ammo.start(position, velocity);

            ammoNode.AddChild(ammo);

            WeaponTimer.Start();

        }

    }

    public void SetWeapon(Weapon purchasedWeapon){

        switch(purchasedWeapon.WEAPONTYPE){

            case Weapon.WeaponClass.white:

                weapons[0] = purchasedWeapon as WhiteWeapon;

                break;

            case Weapon.WeaponClass.primary:

                weapons[1] = purchasedWeapon as FireArm;

                break;

            case Weapon.WeaponClass.secundary:

                weapons[2] = purchasedWeapon as FireArm;

                break;

        }

        purchasedWeapon.Start(this);

        GetNode("Weapons").AddChild(purchasedWeapon);

        EmitSignal("ChangeWeapons");

    }

    public void ChangeWeapon(int weaponName){

        if(currentWeapon != null){

            currentWeapon.Exit();

        }

        currentWeapon = weapons[weaponName];

        currentWeapon.Enter();

        EmitSignal("WeaponInfo", currentWeapon.weaponInfo);

    }

    public void GetCoins(int extraCoins){

        coins += extraCoins;

        EmitSignal("ChangeCoins", coins);

    }

    public void Damage(float damage){

        StatesAnimation.Play("Damage");

        hp -= damage;

        if(hp <= 0){

            hp = 0;

            Death();

        }

        EmitSignal("ChangeHp", hp, maxHp);

    }

    public void Death(){

        GetTree().ChangeScene("res://Menus/MainMenu/MainMenu.tscn");

    }

}
