using Godot;
using System;

public class WeaponStore : InteractiveObject
{

    [Export] PackedScene Weapon;
    [Export] int price;
    Weapon WeaponInstance;

    public Label DamageData;
    public Label FireRateData;
    public Label ScopeData;

    public Label PriceData;
    public TextureButton BuyButton;

    Global Global;


    public override void _Ready()
    {
        base._Ready();

        Global = (Global)GetNode("/root/Global");;

        WeaponInstance = (Weapon)Weapon.Instance();

        DamageData = GetNode<Label>("WeaponInfo/DamageData");

        FireRateData = GetNode<Label>("WeaponInfo/FireRateData");

        ScopeData = GetNode<Label>("WeaponInfo/ScopeData");
        
        PriceData = GetNode<Label>("WeaponInfo/PriceData");

        BuyButton = GetNode<TextureButton>("WeaponInfo/BuyButton");

        DamageData.Text = "Damage: " +  WeaponInstance.DAMAGE.ToString() + " hp";

        FireRateData.Text = "FireRate: " + WeaponInstance.FIRERATE.ToString() + "/Sec";

        PriceData.Text = "$" + price.ToString();

        ScopeData.Text = "Scope :" + WeaponInstance.SCOPE.ToString() + "mts";

        ShowBuyInfo(false);

    }


    public void ShowBuyInfo(bool active){

        GetNode<ColorRect>("WeaponInfo").Visible = active;

        Global.ButtonDisabled(BuyButton, !(active && PlayerTarget.COINS >= price));


    }
    
    public override void _OnInputEvent(Node viewport, InputEvent e, int shapeIdx){  

        bool isPressed = IsObjectPressed(e);

        if(isPressed){

            foreach(Node2D i in GetOverlappingBodies()){

                if(i.IsInGroup("Players")){
                    
                    PlayerTarget = i as Player;

                    break;

                }

            }

            if(PlayerTarget != null){

                ShowBuyInfo(true);

            }

        }

    }

    

    public void _OnBuyButtonPressed(){

        PlayerTarget.SetWeapon(WeaponInstance);

        ShowBuyInfo(false);

        PlayerTarget = null;

    }

    public override void _OnBodyExited(Node body){

        if(body == PlayerTarget){

            PlayerTarget = null;

            ShowBuyInfo(false);

        }

    }


}
