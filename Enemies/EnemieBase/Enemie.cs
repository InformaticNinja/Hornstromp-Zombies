using Godot;
using System;

public class Enemie : KinematicBody2D
{
    EnemieFSM FSM;
    AnimationPlayer StatesAnimation;
    Navigation2D Navigation;
    Player PlayerTarget;
    Timer MovementTimer;
    Godot.Collections.Array<Vector2> navigationPath = new Godot.Collections.Array<Vector2>();

    private float hp;
    public Vector2 velocity = new Vector2();

    public float damage = 10;
    public float speed = 50;
    public int coins;
    float distanceToPath = 0;


    [Signal] public delegate void EnemieDeath();
    [Signal] public delegate void DropCoins(int coins);

    public override void _Ready()
    {
        
        StatesAnimation = GetNode("StatesAnimation") as AnimationPlayer;

        MovementTimer = GetNode<Timer>("MovementTimer");

        FSM = GetNode<EnemieFSM>("EnemieFSM");

        SetTarget();

        SetPath();

    }

    public void Start(float hp, Navigation2D nav, Vector2 spawnPosition, int coins){

        this.hp = hp;

        this.coins = coins;

        Navigation = nav;

        Position = spawnPosition;

    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if(distanceToPath <= 0){

            if(navigationPath.Count > 0){

                SetDirection();

            }

        }

        if(FSM.active){

            FSM.currentState.Update(delta);

        }

        distanceToPath -= (speed * delta);
    }

    public virtual void Damage(float damage, Player playerAttacker){

        StatesAnimation.Play("Damage");

        hp -= damage;

        if(hp <= 0){

            Death(playerAttacker);

        }

    }


    protected virtual void Death(Player playerAttacker){

        Connect("DropCoins", playerAttacker, "GetCoins");

        EmitSignal("DropCoins", coins);

        EmitSignal("EnemieDeath");

        QueueFree();

    }


    public void SetTarget(){

        float shorterDistance = 10000;

        foreach(Player player in GetTree().GetNodesInGroup("Players")){

            float distanceToPlayer = (player.Position - Position).Length();

            if(distanceToPlayer  < shorterDistance ){

                shorterDistance = distanceToPlayer;

                PlayerTarget = player;

            }

        }

    }

    public void SetPath(){

        if(PlayerTarget != null){
            
            navigationPath = new Godot.Collections.Array<Vector2>(Navigation.GetSimplePath(Position, Navigation.GetClosestPoint(PlayerTarget.Position), true));

            if(navigationPath.Count > 0){

                navigationPath.RemoveAt(0);

                SetDirection();

            }

            MovementTimer.Start((float)(.1 * GetTree().GetNodesInGroup("Enemies").Count));

        }

    }

    public void SetDirection(){


            distanceToPath = (navigationPath[0] - Position).Length();
        
            velocity = (navigationPath[0] - Position).Normalized();

            navigationPath.RemoveAt(0);

    }



    public void _OnMovementTimerTimeout(){

        if(navigationPath != null){

            if(navigationPath.Count < 5 || navigationPath[navigationPath.Count -1].DistanceTo(PlayerTarget.Position) > 500){

                SetPath();

            }

        }

        MovementTimer.Start((float)(.1 * GetTree().GetNodesInGroup("Enemies").Count));

    }

    public void _OnHitboxBodyEntered(Node body){

        if(body.IsInGroup("Players")){

            (body as Player).Damage(damage);

        }

    }

}
