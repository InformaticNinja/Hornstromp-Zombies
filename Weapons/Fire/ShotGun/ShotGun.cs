using Godot;
using System;

public class ShotGun : FireArm
{
    Godot.Collections.Array<RayCast2D> shots = new Godot.Collections.Array<RayCast2D>();
    Godot.Collections.Array<AnimatedSprite> explosions = new Godot.Collections.Array<AnimatedSprite>();
    public override void _Ready()
    {
        base._Ready();

        shots.Add(GetNode<RayCast2D>("Shot"));
        shots.Add(GetNode<RayCast2D>("Shot2"));
        shots.Add(GetNode<RayCast2D>("Shot3"));
        shots.Add(GetNode<RayCast2D>("Shot4"));
        shots.Add(GetNode<RayCast2D>("Shot5"));

        explosions.Add(GetNode<AnimatedSprite>("Explosion"));
        explosions.Add(GetNode<AnimatedSprite>("Explosion2"));
        explosions.Add(GetNode<AnimatedSprite>("Explosion3"));
        explosions.Add(GetNode<AnimatedSprite>("Explosion4"));
        explosions.Add(GetNode<AnimatedSprite>("Explosion5"));

        for(int i = 1; i < explosions.Count; i++){

            explosions[i].Connect("animation_finished", this, "_OnExplosionAnimationFinished", new Godot.Collections.Array(new AnimatedSprite[]{explosions[i]}));

        }

    }

    public override void IsShooting(){


        for(int i = 0; i<shots.Count; i++){

            if(shots[i].IsColliding()){

                ShotExplosion(explosions[i], ToLocal(shots[i].GetCollisionPoint()));

                if((shots[i].GetCollider() as Node).IsInGroup("EnemiesHitbox")){

                    ((shots[i].GetCollider() as Area2D).Owner as Enemie).Damage(damage, Player);

                }

            }else{

                ShotExplosion(explosions[i], shots[i].CastTo);

            }

        }

    }

    public override void Attack(Vector2 direction){

        if(checkCharger()){

            if(automatic){

                direction = AutomaticTarget(direction);

                Crosshair.Rotation = direction.Angle();

            }

            float angleDirection = direction.Angle() - Mathf.Pi / 6;

            Vector2 shotsDirections = new Vector2(Mathf.Cos(angleDirection), Mathf.Sin(angleDirection));

            for(int i = 0; i < shots.Count; i++){

                shots[i].CastTo = shotsDirections * scope;

                angleDirection += Mathf.Pi / 12;

                shotsDirections = new Vector2(Mathf.Cos(angleDirection), Mathf.Sin(angleDirection));

            }

        SetPhysicsProcess(true);

        StartCooldown(timeCoolDown);

        }

    }

    public void _OnExplosionAnimationFinished(AnimatedSprite Explosion){

        Explosion.Frame = 0;

        Explosion.Visible = false;

    }

}
