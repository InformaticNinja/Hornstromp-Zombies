using Godot;
using System;

public class GameData : Node2D
{

    Label WeaponInfo;
    public override void _Ready()
    {
        
        WeaponInfo = GetNode<Label>("WeaponInfo");

    }


    public void SetWeaponInfo(String text){

        WeaponInfo.Text = text;

    }

}
