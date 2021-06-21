using Godot;
using System;

public class SplashScreen : Node2D
{
    bool newUser = true;
    Global Global;
    public override void _Ready()
    {

        Global = (Global)GetNode("/root/Global");
        
        newUser = (bool)Global.LoadFileVar("user://data.dat", newUser);

        if(newUser){

            GetTree().ChangeScene("res://World/Maps/Tutorial/TutorialMap.tscn");

            newUser = false;

            Global.SaveFile("user://data.dat", newUser);

        }else{

            GetTree().ChangeScene("res://Menus/MainMenu/MainMenu.tscn");

        }

    }


}
