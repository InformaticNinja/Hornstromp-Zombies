using Godot;
using System;

public class World : Node2D
{
    Player Player;
    Controls Controls;
    GameData GameData;
    MobGenerator MobGenerator;
    public static RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void _Ready()
    {

        rng.Randomize();
        
        InitVars();

        ObjectsConnections();
        
        ObjectsStart();


    }

    public void InitVars(){

        Player = GetNode("Player") as Player;

        Controls = GetNode("UI/Controls") as Controls;

        GameData = GetNode("UI/GameData") as GameData;

        MobGenerator = GetNode("MobGenerator") as MobGenerator;

    }

    public void ObjectsStart(){

        Player.start(GetNode("Ammo") as Node2D);

        //GetNode<TileMap>("NavigableTerrain").UpdateDirtyQuadrants();

        MobGenerator.Start(GetNode<Navigation2D>("NavigableTerrain"));

        Controls.Start(Player);

    }

    public void ObjectsConnections(){

        Player.Connect("WeaponInfo", GameData, "SetWeaponInfo");

        Player.Connect("ChangeCoins", GameData, "SetCoins");

        Player.Connect("ChangeHp", GameData, "SetHp");

        Player.Connect("ChangeWeapons", Controls, "StartButtons");

        MobGenerator.Connect("ChangeRound", GameData, "SetRound");

        MobGenerator.Connect("ChangeZombies", GameData, "SetZombies");

    }

    




}
