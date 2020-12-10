using Godot;
using System;

public class Global : Node
{
       public override void _Ready()
    {
        
    }

    public void ConnectButtons(){

        Godot.Collections.Array buttons = GetTree().GetNodesInGroup("buttons");

        foreach(TextureButton button in buttons){

            if(!IsInGroup("connected")){

                Godot.Collections.Array buttonConnected = new Godot.Collections.Array();

                buttonConnected.Add(button);

                button.Connect("button_down", this, "ButtonDown", buttonConnected);

                button.Connect("button_up", this, "ButtonUp", buttonConnected);

                button.AddToGroup("connected");

            }

        }

    }

    
    public void ButtonDown(TextureButton button){

        button.RectScale = new Vector2(.9f, .9f);


    }

    public void ButtonUP(TextureButton button){

        button.RectScale = new Vector2(1, 1);

    }

    public void ButtonDisabled(TextureButton button, bool disabled){

        button.Disabled = disabled;

        if(disabled)

        button.Modulate = new Color(.3f, .3f, .3f, 1f);

        else

        button.Modulate = new Color(1, 1, 1, 1);
        

    }



}
