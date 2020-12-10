using Godot;
using System;

public class Buttons : Node2D
{
    
    Player Player;

    Godot.Collections.Array<TextureButton> weaponsButtons;

    Global Global;

    public override void _Ready()
    {

        ConnectWeaponsButtons();

        Global = (Global)GetNode("/root/Global");;

    }

    public void ConnectWeaponsButtons(){

        weaponsButtons = new Godot.Collections.Array<TextureButton>(GetNode("Weapons").GetChildren());

        int aux = 0;

        while(aux < weaponsButtons.Count){

            Godot.Collections.Array connectData = new Godot.Collections.Array();

           connectData.Add(weaponsButtons[aux]);

            connectData.Add(aux);

            weaponsButtons[aux].Connect("pressed", this, "_OnWeaponButtonPressed", connectData);

            aux += 1;

        }

    }

    public void Start(Player player){

        Player = player;

        _OnWeaponButtonPressed(weaponsButtons[0], 0);

    }

    
    public void _OnWeaponButtonPressed(TextureButton buttonPressed, int buttonIndex){

        Player.ChangeWeapon(buttonIndex);

        int aux = 0;

        while(aux < weaponsButtons.Count){

            Global.ButtonDisabled((TextureButton)weaponsButtons[aux], weaponsButtons[aux] == null );

            aux += 1;
   
        }

        Global.ButtonDisabled(buttonPressed, true);

    }

}
