using Godot;
using System;

public class AmmoStore : InteractiveObject
{
    Global Global;
    FireArm WeaponAmmo = null;
    public TextureButton BuyButton;

    public override void _Ready(){

        base._Ready();

        Global = (Global)GetNode("/root/Global");;

        BuyButton = GetNode<TextureButton>("WeaponInfo/BuyButton");

        SetPhysicsProcess(false);
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if(PlayerTarget != null){

            if(WeaponAmmo != PlayerTarget.currentWeapon && PlayerTarget.currentWeapon.WEAPONTYPE != Weapon.WeaponClass.white){

                SetAmmoInfo();

            }
        }

    }

        

    public void ShowBuyInfo(bool active){

        GetNode<Node2D>("WeaponInfo").Visible = active;

        Global.ButtonDisabled(BuyButton, !(active && PlayerTarget.COINS >= WeaponAmmo.ammoPrice));


    }

    public void SetAmmoInfo(){

        WeaponAmmo = PlayerTarget.currentWeapon as FireArm;

        GetNode<Label>("WeaponInfo/Price").Text = WeaponAmmo.ammoPrice.ToString();

        GetNode<Label>("WeaponInfo/AmmoCount").Text = WeaponAmmo.chargerSize.ToString() + " shots";

        ShowBuyInfo(true);

    }

    public void _OnBuyButtonPressed(){

        WeaponAmmo.BuyAmmo += WeaponAmmo.chargerSize;

        ShowBuyInfo(false);

        PlayerTarget = null;

    }

    public override void _OnBodyEntered(Node body){

        if(body.IsInGroup("Players")){

            PlayerTarget = body as Player;

            if(PlayerTarget.currentWeapon.WEAPONTYPE != Weapon.WeaponClass.white && PlayerTarget.currentWeapon != null){

                SetAmmoInfo();

            }

            else{

                WeaponAmmo = null;

                 GetNode<Label>("WeaponInfo/Price").Text = "";

                GetNode<Label>("WeaponInfo/AmmoCount").Text = "";


            }

            SetPhysicsProcess(true);

        }

    }

    public override void _OnBodyExited(Node body){

        if(body == PlayerTarget){

            PlayerTarget = null;

            ShowBuyInfo(false);

            SetPhysicsProcess(false);

        }

    }

}
