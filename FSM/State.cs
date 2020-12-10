using Godot;
using System;

public class State : Node
{
    [Signal] public delegate void finished(String state);
    public override void _Ready()
    {
        
    }

    public virtual void Enter(){



    }

    public virtual void Update(float delta){

        

    }

    public virtual void Exit(){


    }


}
