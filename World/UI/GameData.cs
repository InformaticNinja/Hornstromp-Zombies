using Godot;
using System;

public class GameData : Node2D
{

    Label WeaponInfo;
    Label RoundInfo;
    Label ZombiesInfo;
    Label Coins;

    Sprite HpSize;

    int totalZombies = 0;
    public override void _Ready()
    {
        
        WeaponInfo = GetNode<Label>("WeaponInfo");

        RoundInfo = GetNode<Label>("RoundInfo");

        ZombiesInfo = GetNode<Label>("ZombiesInfo");

        Coins = GetNode<Label>("Coins");

        HpSize = GetNode<Sprite>("HpContainer/HpSize");

    }


    public void SetWeaponInfo(String text){

        WeaponInfo.Text = text;
        

    }

    public void SetRound(int round){

        RoundInfo.Text = "Round " + round.ToString();

    }

    public void SetZombies(String message){

        ZombiesInfo.Text = message;

    }

    public void SetZombies(int remainingZombies, int totalZombies ){

         ZombiesInfo.Text = (totalZombies -remainingZombies).ToString() + "/" + totalZombies.ToString();

        

    }

    public void SetCoins(int coins){

        Coins.Text = "$" + coins.ToString();

    }

    public void SetHp(float hp, float maxHP){

        HpSize.RegionRect = new Rect2(0, 0, (200 * hp)/maxHP, 50);

    }

}
