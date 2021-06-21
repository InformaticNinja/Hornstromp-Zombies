using Godot;
using System;

public class BotFollow : BotState
{

    public override void Update(float delta)
    {
        base.Update(delta);


    }

    public override Node2D SetGetTarget(){

        var enemiesArray = GetTree().GetNodesInGroup("Enemies");
        
        float shorterDistance = 1000;

        if(enemiesArray.Count > 0 && target == null){

            foreach(Enemie i in enemiesArray){

                if(Bot.Position.DistanceTo(i.Position) < shorterDistance){

                    shorterDistance = Bot.Position.DistanceTo(i.Position);

                    target = i;

                }

            }

        }

        return target;

    } 

}
