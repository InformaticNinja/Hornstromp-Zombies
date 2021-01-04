using Godot;
using System;

public class Controls : Node2D
{
    

    Player Player;
    Godot.Collections.Array<TextureButton> weaponsButtons;
    Global Global;

    [Signal] public delegate void JoystickInput(String joystick, Vector2 direction);
    [Signal] public delegate void Reload();
    private Sprite joystickMove;
    private Sprite joystickShoot;

    private int moveIndex = -1;
    private int shootIndex = -1;

    public override void _Ready()
    {

        joystickMove = GetNode("JoystickMove") as Sprite;

        joystickShoot = GetNode("JoystickShoot") as Sprite;

        ConnectWeaponsButtons();

        //StartButtons();

        Global = (Global)GetNode("/root/Global");;

        
    }

    public void Start(Player player){

        this.Player = player;

        _OnWeaponButtonPressed(weaponsButtons[0], 0);

        StartButtons();

    }

    

    public override void _Input(InputEvent @event)
    {

        base._Input(@event);

        if(@event is InputEventScreenTouch){ //Comprueba que el evento sea una pulsacion 

            InputEventScreenTouch eventTouch = (InputEventScreenTouch)@event;

            joystickPressed(eventTouch, eventTouch.IsPressed());

        }else if(@event is InputEventScreenDrag){

            InputEventScreenDrag eventDrag = (InputEventScreenDrag)@event;

            if(eventDrag.Index == moveIndex || eventDrag.Index == shootIndex){

                joystickDrag(eventDrag);

            }

        }

    }

    public void joystickPressed( InputEventScreenTouch eventTouch, bool pressed){

        Sprite actualJoystick;

        String joystickString = "";

        if(pressed){
            
            if(joystickMove.GetRect().HasPoint(joystickMove.ToLocal(eventTouch.Position))){

                actualJoystick = joystickMove;

                moveIndex = eventTouch.Index;

                joystickString = "move";

            }

            else if(joystickShoot.GetRect().HasPoint(joystickShoot.ToLocal(eventTouch.Position))){

                actualJoystick = joystickShoot;

                shootIndex = eventTouch.Index;

                joystickString = "shoot";

            }

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

            Sprite direction = actualJoystick.GetNode("Direction") as Sprite;

            direction.Position = Vector2.Zero;

        }

        Player.JoystickPressed(joystickString, pressed, false);
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

        Player.JoystickInput(joystickString, directionPosition.Normalized());
        

    }

    public void StartButtons(){

        for(int i= 0; i < weaponsButtons.Count; i++){

            Global.ButtonDisabled(weaponsButtons[i], Player.weapons[i] == null);

        }

    }

    public void ConnectWeaponsButtons(){

        weaponsButtons = new Godot.Collections.Array<TextureButton>(GetNode("Weapons").GetChildren());

        int aux = 0;

        while(aux < weaponsButtons.Count){

            Godot.Collections.Array connectData = new Godot.Collections.Array();

            connectData.Add(weaponsButtons[aux]);

            connectData.Add(aux);

            weaponsButtons[aux].Connect("pressed", this, "_OnWeaponButtonPressed", connectData);

            aux += 1;

        }

    }

    public void _OnAutoShootPressed(bool pressed){

        Player.JoystickPressed("shoot", pressed, pressed);

    }

    public void _OnReloadPressed(){

        Player.currentWeapon.Reload();

    }

    public void _OnWeaponButtonPressed(TextureButton buttonPressed, int buttonIndex){

        Player.ChangeWeapon(buttonIndex);

        int aux = 0;

        while(aux < weaponsButtons.Count){

            Global.ButtonDisabled((TextureButton)weaponsButtons[aux], Player.weapons[aux] == null );

            aux += 1;
   
        }

        Global.ButtonDisabled(buttonPressed, true);

    }

}
