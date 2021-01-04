using Godot;
using System;

public class MobGenerator : Node2D
{
    
    Navigation2D Navigation;
    Godot.Collections.Array<Sprite> Generators;
    Timer TimerSpawn;
    Godot.Collections.Array<PackedScene> Enemies = new Godot.Collections.Array<PackedScene>();
    public int enemiesPerRound;
    public int remainingEnemies;
    public int remainginEnemiesToKill;
    public int round = 0;
    public float spawnTime = 5;

    public float breakTime = 0;

    [Signal] public delegate void ChangeRound(int round);
    [Signal] public delegate void ChangeZombies(int remaining, int total);

    public override void _Ready()
    {
        
        Generators = new Godot.Collections.Array<Sprite>(GetNode<Node2D>("Generators").GetChildren());

        TimerSpawn = GetNode<Timer>("TimerSpawn");

        PackedScene enemieLoad = GD.Load<PackedScene>("res://Enemies/EnemieBase/EnemieBase.tscn");

        Enemies.Add(enemieLoad);
        
    }


    public void Start(Navigation2D navigation){

        Navigation = navigation;

        StartRound();
        
    }


    public void StartRound(){

        round += 1;

        enemiesPerRound = 10 * round;

        remainingEnemies = enemiesPerRound;

        remainginEnemiesToKill = remainingEnemies;

        TimerSpawn.Connect("timeout", this, "_OnTimerSpawnTimeout");

        TimerSpawn.Start(spawnTime);

        EmitSignal("ChangeRound", round);

        EmitSignal("ChangeZombies", remainginEnemiesToKill, enemiesPerRound);

    }

    public void EnemieDeath(){

        remainginEnemiesToKill -= 1;

        EmitSignal("ChangeZombies", remainginEnemiesToKill, enemiesPerRound);

        if(remainginEnemiesToKill <= 0){

            breakTime = 30;

            TimerSpawn.Start(1);

        }

    }

    public void _OnTimerSpawnTimeout(){

        if(remainingEnemies <= 0){

            TimerSpawn.Disconnect("timeout", this, "_OnTimerSpawnTimeout");

            TimerSpawn.Connect("timeout", this, "_OnTimerBreakTimeout");

        }else{

            Enemie enemieInstance = Enemies[0].Instance() as Enemie;

            Sprite spawn = Generators[World.rng.RandiRange(0, Generators.Count -1)];

            enemieInstance.Start(20 * round, Navigation, spawn.Position, 10);

            enemieInstance.Connect("EnemieDeath", this, "EnemieDeath");

            GetNode<Node2D>("Enemies").AddChild(enemieInstance);

            TimerSpawn.Start(spawnTime);

        }

        remainingEnemies -= 1;

    }

    

    public void _OnTimerBreakTimeout(){

        breakTime -= 1;

        EmitSignal("ChangeZombies", breakTime.ToString() + " remainging");

        if(breakTime > 0){

            TimerSpawn.Start(1);

        }

        else{

            TimerSpawn.Disconnect("timeout", this, "_OnTimerBreakTimeout");

            StartRound();


        }

    }

}
