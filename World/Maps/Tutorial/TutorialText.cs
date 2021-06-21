using Godot;
using System;

public class TutorialText : TextureRect
{
    
    [Export] String text = "";

    RichTextLabel TutorialLabel;
    int index = 0;

    public override void _Ready()
    {
        SetProcess(false);

        TutorialLabel = GetNode<RichTextLabel>("TutorialContainer/TutorialLabel");
    }


    public void Start(String text = ""){

        if(text != ""){

            this.text = text;

        }

        SetProcess(true);

    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        WriteText();

    }

    public void WriteText(){

        if(index < text.Length()){

            TutorialLabel.Text = TutorialLabel.Text + text[index];

            index += 1;

        }


    }

}
