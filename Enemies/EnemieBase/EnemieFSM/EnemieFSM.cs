using Godot;
using System;

public class EnemieFSM : FSM
{
    public new EnemieState currentState;

    public override void _Ready()
    {

        base._Ready();

        initialize();

        currentState = base.currentState as EnemieState;


    }

}
