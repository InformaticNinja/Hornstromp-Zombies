using Godot;
using System;

public class Teleporter : InteractiveObject
{
    [Export] Godot.Collections.Array<Vector2> teleporterPositions = new Godot.Collections.Array<Vector2>();
    
    Godot.Collections.Array<TextureButton> arrowsButtons = new Godot.Collections.Array<TextureButton>();
    Texture arrowTexture = GD.Load<Texture>("res://Pruebas/ArrowButton.png");

    public override void _Ready()
    {

        InitArrows();
        
    }


    public void InitArrows(){

        foreach(Vector2 i in teleporterPositions){

            TextureButton arrowButton = new TextureButton();

            arrowButton.TextureNormal = arrowTexture;

            arrowButton.RectPosition = new Vector2(Mathf.Cos((i - Position).Angle()), Mathf.Sin((i - Position).Angle())) * 64;

            arrowButton.RectRotation = Mathf.Rad2Deg((i - Position).Angle());

            arrowsButtons.Add(arrowButton);

            GetNode<Node2D>("Directions").AddChild(arrowButton);

            Godot.Collections.Array sendData = new Godot.Collections.Array();

            sendData.Add(arrowButton);

            sendData.Add(i);

            arrowButton.Connect("pressed", this, "_OnArrowPressed", sendData);

        }

        ShowArrows(false);

    }

    public void ShowArrows(bool show){

        GetNode<Node2D>("Directions").Visible = show;

        foreach(TextureButton button in arrowsButtons){

            button.Disabled = !show;

        }

    }


    public override void _OnInputEvent(Node viewport, InputEvent e, int shapeIdx){

        bool isPressed = IsObjectPressed(e);
        
        if(isPressed){

            foreach(Node2D i in GetOverlappingBodies()){

                if(i.IsInGroup("Players")){
                    
                    PlayerTarget = i as Player;

                    break;

                }

            }

            if(PlayerTarget != null){

                ShowArrows(true);

            }

        }

    }

    public void _OnArrowPressed(TextureButton arrowButton, Vector2 teleportPosition){

        PlayerTarget.Position = teleportPosition;

        PlayerTarget = null;

        ShowArrows(false);

    }

    

    public override void _OnBodyExited(Node body)
    {
        
        if(body == PlayerTarget){

            ShowArrows(false);

            PlayerTarget = null;

        }

    }


}
