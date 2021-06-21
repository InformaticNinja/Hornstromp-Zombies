using Godot;
using System;

public class BotState : State
{
    protected Bot Bot;
    protected Node2D target;
    public override void _Ready(){

        Bot = Owner as Bot;
        
    }
    public override void Update(float delta){

        Bot.MoveAndSlide(Bot.velocity);

    }

    public virtual Node2D SetGetTarget(){

        return target;
    }




}
