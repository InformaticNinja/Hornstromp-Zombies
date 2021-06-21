using Godot;
using System;

public class PlayerFSM : FSM
{

    public new PlayerState currentState;

    public override void _Ready()
    {

        base._Ready();

        initialize();

        currentState = base.currentState as PlayerState;
        
    }

    

    

    
}
