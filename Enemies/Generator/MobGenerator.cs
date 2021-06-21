using Godot;
using System;

public class MobGenerator : Node2D
{
    
    [Export] int initialEnemies;
    [Export] int initialHp;
    Navigation2D Navigation;
    Godot.Collections.Array<Sprite> Generators;
    Timer TimerSpawn;
    Godot.Collections.Array<PackedScene> Enemies = new Godot.Collections.Array<PackedScene>();
    Godot.Collections.Array<Enemie> EnemiesAlive = new Godot.Collections.Array<Enemie>();
    Godot.Collections.Array<Bot> BotsAlive = new Godot.Collections.Array<Bot>();

    private int enemiesIterator = 0;
    private int botsIterator = 0;
    private bool enemiePath = false;
    public int enemiesPerRound;
    public int remainingEnemies;
    public int remainginEnemiesToKill;
    public int round = 0;
    public float spawnTime = 5;
    public float breakTime = 0;

    [Signal] public delegate void ChangeRound(int round);
    [Signal] public delegate void ChangeZombies(int remaining, int total);
    [Signal] public delegate void NewEnemie(Enemie newEnemie);
    [Signal] public delegate void RemoveEnemie(Enemie removeEnemie);

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

        enemiesPerRound = initialEnemies * round;

        remainingEnemies = enemiesPerRound;

        remainginEnemiesToKill = remainingEnemies;

        TimerSpawn.Connect("timeout", this, "_OnTimerSpawnTimeout");

        TimerSpawn.Start(spawnTime);

        EmitSignal("ChangeRound", round);

        EmitSignal("ChangeZombies", remainginEnemiesToKill, enemiesPerRound);

    }

    public void EnemieDeath(Enemie enemieDeath){
        
        remainginEnemiesToKill -= 1;

        int removeIndex = EnemiesAlive.IndexOf(enemieDeath);

        EnemiesAlive.RemoveAt(removeIndex);

        if(removeIndex <= enemiesIterator && enemiesIterator > 0){

            enemiesIterator -=1;

        }

        enemiesIterator = 0;

        EmitSignal("ChangeZombies", remainginEnemiesToKill, enemiesPerRound);

        EmitSignal("RemoveEnemie", enemieDeath);

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

            enemieInstance.Start(initialHp * round, Navigation, spawn.Position, 10);

            enemieInstance.Connect("EnemieDeath", this, "EnemieDeath", new Godot.Collections.Array(new Enemie[]{enemieInstance}));

            EnemiesAlive.Add(enemieInstance);

            GetNode<Node2D>("Enemies").AddChild(enemieInstance);

            TimerSpawn.Start(spawnTime);

            GetNode<Timer>("TimerPath").Start();

            EmitSignal("NewEnemie", enemieInstance);

        }

        remainingEnemies -= 1;

    }

    public void SetEnemiePath(){

        EnemiesAlive[enemiesIterator].SetPath();

        enemiesIterator++;

        if(enemiesIterator == EnemiesAlive.Count){

            enemiesIterator = 0;

        }

    }

    public void SetBotPath(){

        BotsAlive[botsIterator].SetPath();

        botsIterator++;

        if(botsIterator == BotsAlive.Count){

            enemiesIterator = 0;

        }

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

    public void _OnTimerPathTimeout(){

        if(enemiePath){

            if(EnemiesAlive.Count > 0){

                SetEnemiePath();

            }

        }else{

            if(BotsAlive.Count > 0){

                SetBotPath();

            }

        }

        GetNode<Timer>("TimerPath").Start();

        enemiePath = !enemiePath;

    }



}
