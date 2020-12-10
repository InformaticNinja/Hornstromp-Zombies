using Godot;
using System;

public class World : Node2D
{
    Player Player;
    Controls Controls;
    GameData GameData;
    Buttons Buttons;
    public override void _Ready()
    {
        
        InitVars();
        
        PlayerStart();

        UIStart();

    }

    public void InitVars(){

        Player = GetNode("Player") as Player;

        Buttons = GetNode("UI/Buttons") as Buttons;

        Controls = GetNode("UI/Controls") as Controls;

        GameData = GetNode("UI/GameData") as GameData;

    }

    public void PlayerStart(){

        Player.start(GetNode("Ammo") as Node2D);

        Player.Connect("WeaponInfo", GameData, "SetWeaponInfo");

    }

    

    public void UIStart(){

        Buttons.Start(Player);

        Controls.Connect("JoystickInput", Player, "JoystickInput");

        Controls.Connect("JoystickPressed", Player, "JoystickPressed");

    }




}
