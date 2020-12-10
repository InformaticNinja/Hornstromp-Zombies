using Godot;
using System;

public class FSM : Node
{
    [Export] String initialState;


    public State currentState;

    public Godot.Collections.Dictionary<String, State> statesMap = new Godot.Collections.Dictionary<String, State>();

    public Godot.Collections.Array<State> statesStack = new Godot.Collections.Array<State>();

    public bool active = false;
    

    public override void _Ready()
    {
        
        foreach(State i in GetChildren() ){
            
            statesMap.Add(i.Name, i);

            i.Connect("finished", this, "ChangeState");

        }

    }

    public void initialize(){

        statesStack.Add(statesMap[initialState]);

        currentState = statesStack[0];

        GD.Print(currentState);

        currentState.Enter();

        active = true;

    }

    public void ChangeState(String stateName){

        currentState.Exit();

        statesStack[0] = statesMap[stateName];

        currentState = statesStack[0];

        currentState.Enter();

    }

}
