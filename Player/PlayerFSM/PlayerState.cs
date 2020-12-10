using Godot;
using System;

public class PlayerState : State
{
    
    Player Player;
    public override void _Ready()
    {

        Player = Owner as Player;
        
    }


    public override void Update(float delta){
        
        Player.MoveAndSlide(Player.velocity * Player.speed);

    }

    public void JoystickMove(Vector2 direction){

        Player.velocity = direction;

    }


}
