using Godot;
using System;

public class Sniper : FireArm
{
    public override void Enter(){

        base.Enter();

        Player.CameraZoom(1.5f);

    }

    public override void Exit()
    {
        base.Exit();

        Player.CameraZoom(1);
    }
}
