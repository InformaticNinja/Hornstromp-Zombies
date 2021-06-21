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
    private Tween TweenNode;
    private Rect2 moveRect = new Rect2(new Vector2(0, 192), new Vector2(640, 528));
    private Rect2 shootRect = new Rect2(new Vector2(640, 192), new Vector2(640, 528));
    private Vector2 moveInitialPos = new Vector2(200, 544);
    private Vector2 shootInitialPos = new Vector2(1064, 440);

    private int moveIndex = -1;
    private int shootIndex = -1;

    public Rect2 MOVERECT{get => moveRect; set => moveRect = value;}
    public Rect2 SHOOTRECT{get => shootRect; set => shootRect = value;}

    public override void _Ready()
    {

        joystickMove = GetNode("JoystickMove") as Sprite;

        joystickShoot = GetNode("JoystickShoot") as Sprite;

        TweenNode = GetNode("Tween") as Tween;

        ConnectWeaponsButtons();

        //StartButtons();

        Global = (Global)GetNode("/root/Global");

        
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

        Sprite actualJoystick = null;

        String joystickString = "";

        if(pressed){

            if(moveRect.HasPoint(eventTouch.Position)){

                joystickMove.Position = eventTouch.Position;

                actualJoystick = joystickMove;

                moveIndex = eventTouch.Index;

                joystickString = "move";

            }

            else if(shootRect.HasPoint(eventTouch.Position)){

                joystickShoot.Position = eventTouch.Position;

                actualJoystick = joystickShoot;

                shootIndex = eventTouch.Index;

                joystickString = "shoot";

            }

        }else{

            if(eventTouch.Index == moveIndex){

                joystickMove.Position = moveInitialPos;

                moveIndex = -1;

                actualJoystick = joystickMove;

                joystickString = "move";

            }else if(eventTouch.Index == shootIndex){

                Vector2 directionPosition = joystickShoot.ToLocal(eventTouch.Position);

                joystickShoot.Position = shootInitialPos;

                shootIndex = -1;

                actualJoystick = joystickShoot;

                joystickString = "shoot";

                if(directionPosition.Length() <= 50){

                    Player.JoystickPressed("shoot", pressed, true);

                }

            }

            if(actualJoystick != null){

                Sprite direction = actualJoystick.GetNode("Direction") as Sprite;

                direction.Position = Vector2.Zero;

            }

        }

        if(joystickString != ""){

            Player.JoystickPressed(joystickString, pressed, false);

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

        if(directionPosition.Length() > 50){

            direction.Position = directionPosition;

            Player.JoystickInput(joystickString, directionPosition.Normalized());

        }else{

            direction.Position = Vector2.Zero;

        }


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

    public void StartAttackCooldown(float coolTime){

        if(TweenNode.IsActive()){

            TweenNode.StopAll();

        }

        TweenNode.InterpolateProperty(joystickShoot, "modulate", new Color(0, 0, 0, 1), joystickShoot.Modulate, coolTime, Tween.TransitionType.Elastic);

        TweenNode.Start();

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
