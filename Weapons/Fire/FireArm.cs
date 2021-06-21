using Godot;
using System;

public class FireArm : Weapon
{
    [Export] protected float timeCharger;
    [Export]  public int chargerSize;
    [Export] protected int totalBullets;
    
    [Export] public int ammoPrice;
    protected int bulletsInCharger;
    protected int bulletsRemaining;
    public Sprite Crosshair;
    public AnimatedSprite Explosion;
    public RayCast2D Shot;
    protected bool recharge = false;

    public int BuyAmmo{get => bulletsRemaining; set {bulletsRemaining = value; Mathf.Clamp(bulletsRemaining, 0, totalBullets);}}
    

    public override void _Ready()
    {
        base._Ready();

        Crosshair = GetNode("Crosshair") as Sprite;
        Explosion = GetNode("Explosion") as AnimatedSprite;
        Shot = GetNode("Shot") as RayCast2D;
        WeaponSprite = GetNode<Sprite>("Sprite");
        Crosshair.RegionRect = new Rect2(0, 0, scope, 8);
        bulletsInCharger = chargerSize;
        bulletsRemaining = totalBullets;
        weaponInfo = bulletsInCharger.ToString() + "/" + bulletsRemaining.ToString() ;
        
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        IsShooting();

        SetPhysicsProcess(false);

    }

    public virtual void IsShooting(){

        if(Shot.IsColliding()){
            
            ShotExplosion(Explosion, ToLocal(Shot.GetCollisionPoint()));

            if((Shot.GetCollider() as Node).IsInGroup("EnemiesHitbox")){

                ((Shot.GetCollider() as Area2D).Owner as Enemie).Damage(damage, Player);

            }

        }else{

            ShotExplosion(Explosion, Shot.CastTo);

        }

    }

    public override void JoystickPressed(bool pressed, bool auto = false)
    {
        base.JoystickPressed(pressed, auto);

        Crosshair.Visible = pressed;

        if(auto && CoolDown.IsStopped()){

            Attack(attackDirection);

        }

    }

    public override void Aim(Vector2 direction){
        
        if(direction != Vector2.Zero){

            SetWeaponDirection(direction);
        }

        if(CoolDown.IsStopped()){

            Attack(attackDirection);

        }

    }

    public override void Attack(Vector2 direction){

        if(checkCharger()){

            if(automatic){

                direction = AutomaticTarget(direction);

                Crosshair.Rotation = direction.Angle();

            }

            Shot.CastTo = direction * scope;

        SetPhysicsProcess(true);

        StartCooldown(timeCoolDown);

        base.Attack(direction);

        }

    }


    public bool checkCharger(){

        if(bulletsInCharger > 0){

            bulletsInCharger -= 1;

            weaponInfo = bulletsInCharger.ToString() + "/" + bulletsRemaining.ToString() ;

            Player.EmitSignal("WeaponInfo", weaponInfo);

            return true;

        }else if(bulletsRemaining > 0 && !recharge){

            Reload();

        }

        return false;

    }


    public override void Reload(){

        recharge = true;

        StartCooldown(timeCharger);
    }

    public void ShotExplosion(AnimatedSprite explosion, Vector2 position){

        explosion.Position = position;

        explosion.Visible = true;

        explosion.Play();

    }

    public override void SetWeaponDirection(Vector2 direction){

        Crosshair.Rotation = direction.Angle();

        base.SetWeaponDirection(direction);

    }

    public void _OnExplosionAnimationFinished(){

        Explosion.Frame = 0;

        Explosion.Visible = false;

    }

    public override void _OnCoolDownTimeout()
    {
        base._OnCoolDownTimeout();

        if(recharge){

            var aux = bulletsRemaining >= chargerSize - bulletsInCharger ? chargerSize - bulletsInCharger  : bulletsRemaining;

            bulletsInCharger += aux;

            bulletsRemaining -= aux;

            weaponInfo = bulletsInCharger.ToString() + "/" + bulletsRemaining.ToString() ;

            recharge = false;

            Player.EmitSignal("WeaponInfo", weaponInfo);

        }

        if(isAttacking){
            
            Attack(attackDirection);

        }
    }


}
