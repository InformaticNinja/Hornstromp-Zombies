using Godot;
using System;

public class MainMenu : CanvasLayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _OnPlayPressed(){

        GetTree().ChangeScene("res://World/Maps/Map1/Map1.tscn");

    }

}
