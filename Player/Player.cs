using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export] Godot.Collections.Array<Weapon> weapons = new Godot.Collections.Array<Weapon>();
    [Signal] public delegate void WeaponInfo(String weaponInfo);
    PackedScene ammoLoad;
    public Vector2 velocity = new Vector2();
    public float speed = 500f;
    Node2D ammoNode;
    PlayerFSM FSM;
    Timer WeaponTimer;
    Weapon currentWeapon;

    

    public override void _Ready()
    {

        weapons[0] = (GD.Load("res://Weapons/White/Knife/Knife.tscn") as PackedScene).Instance() as WhiteWeapon;

        weapons[1] = (GD.Load("res://Weapons/Fire/Pistol/Pistol.tscn") as PackedScene).Instance() as FireArm;

        weapons[2] = (GD.Load("res://Weapons/Fire/Metra/Metra.tscn") as PackedScene).Instance() as FireArm;

        weapons[0].Start(this);

        weapons[1].Start(this);

        weapons[2].Start(this);

        GetNode("Weapons").AddChild(weapons[0]);

        GetNode("Weapons").AddChild(weapons[1]);

        GetNode("Weapons").AddChild(weapons[2]);

        FSM = GetNode("PlayerFSM") as PlayerFSM;

        ammoLoad = GD.Load("res://Weapons/Ammo/Ammo.tscn") as PackedScene;

        WeaponTimer = GetNode("WeaponTimer") as Timer;
        
    }

    public void start(Node2D ammoNode){

        this.ammoNode = ammoNode;

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

    public void JoystickPressed(String joystick, bool pressed){

        if(joystick == "shoot"){

            currentWeapon.JoystickPressed(pressed);

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

    public void ChangeWeapon(int weaponName){

        if(currentWeapon != null){

            currentWeapon.Exit();

        }

        currentWeapon = weapons[weaponName];

        currentWeapon.Enter();

        EmitSignal("WeaponInfo", currentWeapon.weaponInfo);

    }

}
