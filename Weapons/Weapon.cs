using Godot;
using System;

public class Weapon : Node2D
{

    [Export] protected float damage;
    [Export] protected float timeCoolDown;
    [Export] protected int scope;
    [Export] protected WeaponClass weaponType;
    protected Timer CoolDown;
    protected Sprite WeaponSprite;
    protected Vector2 attackDirection;
    protected bool isAttacking = false;
    public String weaponInfo;
    protected Area2D AutomaticArea;
    protected Player Player;
    protected bool automatic = false;


    public enum WeaponClass{white, primary, secundary}

    public float DAMAGE{ get => this.damage; set => this.damage = value;}
    public float FIRERATE{ get=> 1/this.timeCoolDown; set => this.timeCoolDown = value;}
    public int SCOPE{ get=> scope; set => this.scope = value;}

    public WeaponClass WEAPONTYPE {get => weaponType;}

    public override void _Ready()
    {
        CoolDown = GetNode("CoolDown") as Timer;

        SetPhysicsProcess(false);

        Visible = false;

        AutomaticArea = GetNode("AutomaticArea") as Area2D;

        CollisionShape2D CollisionAutomatic = AutomaticArea.GetNode<CollisionShape2D>("Collision");

        CollisionAutomatic.Shape = new CircleShape2D();

        (CollisionAutomatic.Shape as CircleShape2D).Radius = scope;
    }

    public void Start(Player player){

        Player = player;

    }

    public virtual void Attack(Vector2 direction){}

    public virtual void JoystickPressed(bool pressed, bool auto = false){

        isAttacking = pressed;

        automatic = auto;

        if(!pressed){

            attackDirection = Vector2.Zero;

        }

        if(auto && CoolDown.IsStopped()){

            Attack(attackDirection);

        }
    }

    public virtual void Aim(Vector2 direction){}

    public virtual void Reload(){}

    public virtual Vector2 AutomaticTarget(Vector2 direction){

        Godot.Collections.Array<Node2D> enemiesOptions = new Godot.Collections.Array<Node2D>(AutomaticArea.GetOverlappingAreas());

        if(enemiesOptions.Count > 0){

            Enemie target = (enemiesOptions[World.rng.RandiRange(0, enemiesOptions.Count -1)].Owner) as Enemie;

            direction = (target.GlobalPosition - Player.GlobalPosition).Normalized(); 

            if(Player.velocity != Vector2.Zero){

                float dispersion = Mathf.Deg2Rad(World.rng.RandfRange(-30, 30));

                direction += new Vector2(Mathf.Cos(dispersion), Mathf.Sin(dispersion));

                direction = direction.Normalized();

            }

        }else{

            float randomTarget = Mathf.Deg2Rad(World.rng.RandiRange(0, 360));

            direction = new Vector2(Mathf.Cos(randomTarget), Mathf.Sin(randomTarget));

        }

        return direction;

    }

    public virtual void SetWeaponDirection(Vector2 direction){
        
        bool flip = true ;

        if(direction.Angle() <= Mathf.Pi / 2 && direction.Angle() >= Mathf.Pi / -2){

            flip = false;

        }

        WeaponSprite.Rotation = direction.Angle();

        if( WeaponSprite.FlipV != flip){

            WeaponSprite.FlipV = !WeaponSprite.FlipV;

            WeaponSprite.Rotation = direction.Angle() - Mathf.Pi;

            WeaponSprite.Offset = new Vector2(WeaponSprite.Offset.x, WeaponSprite.Offset.y * -1);

        }
            
        attackDirection = direction;

    }

    public void StartCooldown(float time){

        CoolDown.Start(time);

        Player.EmitSignal("Cooldown", time);

    }

    public virtual void Enter(){

        Visible = true;

    }

    public virtual void Exit(){

        Visible = false;

    }


    public virtual void _OnCoolDownTimeout(){ 
    }

}
