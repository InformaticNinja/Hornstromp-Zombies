using Godot;
using System;

public class InteractiveObject : Area2D
{
    [Export] Vector2 clickableSize = new Vector2();
    protected AnimatedSprite AnimatedSprite;
    protected Player PlayerTarget;

    public override void _Ready()
    {
        AnimatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }


    public bool IsObjectPressed(InputEvent e){

        bool objectPressed = false;

        if(e is InputEventScreenTouch){

            InputEventScreenTouch touchEvent = e as InputEventScreenTouch;

            Vector2 eventPosition =  ToLocal(GetCanvasTransform().AffineInverse().Xform(touchEvent.Position)); 

            objectPressed = eventPosition.x > - (clickableSize.x / 2) && eventPosition.x < (clickableSize.x / 2) && eventPosition.y > - (clickableSize.y / 2) && eventPosition.y < (clickableSize.y / 2);

        }

        return objectPressed;

    }

    public virtual void _OnInputEvent(Node viewport, InputEvent e, int shapeIdx){  }

    public virtual void _OnBodyExited(Node body){}

}
