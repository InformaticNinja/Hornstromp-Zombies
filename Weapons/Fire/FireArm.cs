using Godot;
using System;

public class FireArm : Weapon
{
    [Export] protected float timeCharger;
    [Export]  protected int chargerSize;
    [Export] protected int totalBullets;
    protected int bulletsInCharger;
    protected int bulletsRemaining;
    [Export] protected int scope;
    public Sprite Crosshair;
    public AnimatedSprite Explosion;
    public RayCast2D Shot;

    protected bool recharge = false;


    public override void _Ready()
    {

        base._Ready();

        Crosshair = GetNode("Crosshair") as Sprite;
        Explosion = GetNode("Explosion") as AnimatedSprite;
        Shot = GetNode("Shot") as RayCast2D;
        Crosshair.RegionRect = new Rect2(0, 0, scope, 8);

        bulletsInCharger = chargerSize;

        bulletsRemaining = totalBullets;

        weaponInfo = bulletsInCharger.ToString() + "/" + totalBullets.ToString() ;
        
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if(Shot.IsColliding()){

            ShotExplosion(ToLocal(Shot.GetCollisionPoint()));

            if((Shot.GetCollider() as Node).IsInGroup("Enemies")){

                (Shot.GetCollider() as Enemie).Damage();

            }

        }else{

            ShotExplosion(Shot.CastTo);

        }

        SetPhysicsProcess(false);

    }

    public override void JoystickPressed(bool pressed)
    {
        base.JoystickPressed(pressed);

        Crosshair.Visible = pressed;

    }

    public override void Aim(Vector2 direction){
        
        if(direction != Vector2.Zero){
            
            if(!Crosshair.Visible){
                
                Crosshair.Visible = true;

            }

            Crosshair.Rotation = direction.Angle();

            attackDirection = direction;

        }

    }


    public bool checkCharger(){

        if(bulletsInCharger > 0){

            bulletsInCharger -= 1;

            weaponInfo = bulletsInCharger.ToString() + "/" + totalBullets.ToString() ;

            Player.EmitSignal("WeaponInfo", weaponInfo);

            return true;

        }else{

            recharge = true;

            CoolDown.Start(timeCharger);

            return false;

        }

    }

    public void ShotExplosion(Vector2 position){

        Explosion.Position = position;

        Explosion.Visible = true;

        Explosion.Play();

    }

    public void _OnExplosionAnimationFinished(){

        Explosion.Frame = 0;

        Explosion.Visible = false;

    }

    public override void _OnCoolDownTimeout()
    {
        base._OnCoolDownTimeout();

        if(recharge){

            bulletsInCharger = chargerSize;

            totalBullets -= chargerSize;

            weaponInfo = bulletsInCharger.ToString() + "/" + totalBullets.ToString() ;

            recharge = false;

        }
    }


}
