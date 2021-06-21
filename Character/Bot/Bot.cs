using Godot;
using System;

public class Bot : Character
{
    Navigation2D NavigationMap;
    Godot.Collections.Array<Vector2> navigationPath = new Godot.Collections.Array<Vector2>();
    protected BotFSM BotFSM;

    float distanceToPath = 0;
    public void Start(Navigation2D nav){

        NavigationMap = nav;

    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
    }
    public void SetPath(){

        Node2D target =  BotFSM.currentState.SetGetTarget();

        navigationPath = new Godot.Collections.Array<Vector2>(NavigationMap.GetSimplePath(Position, target.Position, true));

        if(navigationPath.Count > 0){

            navigationPath.RemoveAt(0);

            SetDirection();

        }

    }

    public void SetDirection(){

        distanceToPath = Position.DistanceTo(navigationPath[0]);

        velocity = (navigationPath[0] - Position).Normalized();

        navigationPath.RemoveAt(0);

    }


}
