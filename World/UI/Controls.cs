using Godot;
using System;

public class Controls : Node2D
{
    
    [Signal] public delegate void JoystickInput(String joystick, Vector2 direction);
    [Signal] public delegate void JoystickPressed(String joystick, bool pressed);
    private Sprite joystickMove;
    private Sprite joystickShoot;

    private int moveIndex = -1;
    private int shootIndex = -1;

    public override void _Ready()
    {

        joystickMove = GetNode("JoystickMove") as Sprite;
        joystickShoot = GetNode("JoystickShoot") as Sprite;
        
    }

    public override void _Input(InputEvent @event)
    {

        base._Input(@event);

        if(@event is InputEventScreenTouch){ //Comprueba que el evento sea una pulsacion 

            InputEventScreenTouch eventTouch = (InputEventScreenTouch)@event;

            joystickPressed(eventTouch, eventTouch.IsPressed());
            //joystickPressed(actualJoystick, eventTouch, eventTouch.IsPressed());

        }else if(@event is InputEventScreenDrag){

            InputEventScreenDrag eventDrag = (InputEventScreenDrag)@event;

            joystickDrag(eventDrag);

        }

        
    }

    public void joystickPressed( InputEventScreenTouch eventTouch, bool pressed){

        Sprite actualJoystick;

        String joystickString = "";

        if(pressed){

            if(eventTouch.Position.x < 640){

                actualJoystick = joystickMove;

                moveIndex = eventTouch.Index;

                joystickString = "move";

            }

            else{

                actualJoystick = joystickShoot;

                shootIndex = eventTouch.Index;

                joystickString = "shoot";

            }

            actualJoystick.Position = ToLocal(eventTouch.Position);

            Sprite direction = actualJoystick.GetNode("Direction") as Sprite;

            direction.Position = Vector2.Zero;
            
            

        }else{

            if(eventTouch.Index == moveIndex){

                moveIndex = -1;

                actualJoystick = joystickMove;

                joystickString = "move";

            }else{

                shootIndex = -1;

                actualJoystick = joystickShoot;

                joystickString = "shoot";

            }

        }

        if(eventTouch.Position.y > 192 || !pressed){

            EmitSignal("JoystickPressed", joystickString, pressed);

            actualJoystick.Visible = pressed;

        }

        

    }


    public void joystickDrag(InputEventScreenDrag eventDrag){
        
        Sprite actualJoystick = null;

        Sprite direction;

        String joystickString = "";

        if(eventDrag.Index == moveIndex){

            joystickString = "move";

            actualJoystick = joystickMove;

        }else if(eventDrag.Index == shootIndex){

            joystickString = "shoot";

            actualJoystick = joystickShoot;

        }

        direction = actualJoystick.GetNode("Direction") as Sprite;

        Vector2 directionPosition = actualJoystick.ToLocal(eventDrag.Position);

        if(directionPosition.Length() > 125){

            directionPosition = directionPosition.Normalized() * 125;

        }

        direction.Position = directionPosition;

        EmitSignal("JoystickInput", joystickString, directionPosition.Normalized());
        

    }

}
