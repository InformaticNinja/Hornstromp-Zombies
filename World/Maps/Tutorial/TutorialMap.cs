using Godot;
using System;

public class TutorialMap : World
{

    PackedScene tutorialTextLoad;
    CanvasLayer Tutorials;
    Node2D TutorialTerrain;

    String[] texts = new String[]{
        "Hola mi nombre es Hornstromp y el dia de hoy sere tu guia en este campo de entrenamiento. \nPara iniciar debes tomar el celular en posicion horizontal tal y como se muestra en la imagen de referencia",
        "Puedes caminar presionando el controlador azul y arrastrando tu dedo a la direccion deseada de movimiento.\n \nMuevete hasta llegar a la zona verde",
        "Puedes atacar presionando el controlador rojo y arrastrandolo en la dirrecion de ataque deseada.\nRecuerda arrastrarlo lo suficiente",
        "Ahora prueba a dar un disparo automatico, Para ello tienes que presionar el boton central del controlador rojo y soltarlo.\nEsto dara un disparo automatico al enemigo mas cercano, pero ojo siempre que el arma tenga suficiente alcance.\nPara ello parate dentro del circulo rojo y despues efectua el disparo automatico",
        "Perfecto, aprendes muy rapido. \nComo puedes notar al matar un enemigo este te dara dinero con el que puedes comprar armas, municion e incluso salud. Para ello baja a la siguiente habitacion para comprar una bonita pistola :D",
        "Para poder comprar un arma tienes que pararte cerca de la tienda y despues dar click en la tienda. A continuacion se te abrira una pequeña ventana con informacion del arma como el dano, disparos por segundo, alcance y el precio.\nPresiona el boton para comprar ",
        "Perfecto, ahora como notaras tienes 3 botones nuevos para cambiar entre tu arma de cuerpo, arma principal y arma secundaria. Solo presiona el boton para cambiar.\nCon las armas de fuego puedes ver cuanta municion tienes en cargador y cuanta en total\nCamina a la siguiente sala para terminar tu entrenamiento",
        "Esos spawns que ves ahi son generadores de zombies, los zombies se generan por rondas cada cierto tiempo. \nEn la parte superior izquierda puedes ver informacion de la ronda, de cuantos zombies has matado y el total de zombies\nMatalos a todos!!!  ",
        "Eres un maestro, has terminado la prueba.\nEs momento de ir a el campo de batalla real"
    };
    Rect2 shootRect;
    Rect2 moveRect;
    public override void _Ready()
    {

        base._Ready();

        Tutorials = GetNode<CanvasLayer>("Tutorials");
        tutorialTextLoad = GD.Load<PackedScene>("res://World/Maps/Tutorial/TutorialText.tscn");
        TutorialTerrain = GetNode<Node2D>("TutorialTerrain");
        FirstTutorial();


    }
    public override void ObjectsStart(){

        Player.start(GetNode("Ammo") as Node2D);

        foreach(CanvasItem i in Controls.GetChildren()){

            i.Visible = false;

        }

        foreach(CanvasItem i in GameData.GetChildren()){

            i.Visible = false;

        }

        shootRect = Controls.SHOOTRECT;
        moveRect = Controls.MOVERECT;
        Controls.SHOOTRECT = new Rect2(0,0,0,0);
        Controls.MOVERECT = new Rect2(0,0,0,0);

    }

    public TutorialText InitTutorialText(int index){

        TutorialText tutorialTextInstane = tutorialTextLoad.Instance() as TutorialText;

        Tutorials.AddChild(tutorialTextInstane);

        tutorialTextInstane.Start(texts[index]);

        return tutorialTextInstane;

    }

    public async void FirstTutorial(){

        var firstTutorial = InitTutorialText(0);

        await ToSignal(GetTree().CreateTimer(13), "timeout");

        firstTutorial.QueueFree();

        SecondTutorial();

    }

    public void SecondTutorial(){

        var secondTutorial =  InitTutorialText(1);

        Node2D tutorialMove = (GD.Load("res://World/Maps/Tutorial/TutorialMove.tscn") as PackedScene).Instance() as Node2D;

        tutorialMove.GetNode<Area2D>("Area2D").Connect("input_event", this, "MoveEnabled", new Godot.Collections.Array(new Node2D[]{tutorialMove}));

        Tutorials.AddChild(tutorialMove);

        Controls.Start(Player);

        Area2D tutorialWalk = (GD.Load("res://World/Maps/Tutorial/TutorialWalk.tscn") as PackedScene).Instance() as Area2D;

        TutorialTerrain.AddChild(tutorialWalk);

        tutorialWalk.Position = new Vector2(1216, 336);

        Godot.Collections.Array sendData = new Godot.Collections.Array();

        sendData.Add(secondTutorial);

        sendData.Add(tutorialWalk);

        tutorialWalk.Connect("body_entered", this, "ThirdTutorial", sendData);

        Controls.MOVERECT = moveRect;

    }

    public void MoveEnabled(Node viewport, InputEvent e, int shape, Node2D tutorialMove){

        if(e.IsPressed()){

            tutorialMove.QueueFree();

            Controls.GetNode<Sprite>("JoystickMove").Visible = true;

        }

    }

    public void ThirdTutorial(Node2D body, TutorialText removeTutorial, Area2D removeArea){

        removeTutorial.QueueFree();

        removeArea.CallDeferred("queue_free");

        var thirdTutorial =  InitTutorialText(2);

        Node2D tutorialShoot = (GD.Load("res://World/Maps/Tutorial/TutorialShoot.tscn") as PackedScene).Instance() as Node2D;

        tutorialShoot.GetNode<Area2D>("Area2D").Connect("input_event", this, "FourthTutorial", new Godot.Collections.Array(new Node[]{thirdTutorial, tutorialShoot}));

        Tutorials.CallDeferred("add_child", tutorialShoot);

        Controls.SHOOTRECT = shootRect;

    }

    public void FourthTutorial(Node viewport, InputEvent e, int shape, Node removeTutorial, Node removeTutorial2){

        if(e.IsPressed()){

            removeTutorial.QueueFree();

            removeTutorial2.QueueFree();

            Controls.GetNode<Sprite>("JoystickShoot").Visible = true;

            var fourthTutorial =  InitTutorialText(3);

            Node2D tutorialAutoShoot = (GD.Load("res://World/Maps/Tutorial/TutorialAutoShoot.tscn") as PackedScene).Instance() as Node2D;

            Enemie enemieTutorial = GD.Load<PackedScene>("res://Enemies/EnemieBase/EnemieBase.tscn").Instance() as Enemie;

            Sprite RedCircle = new Sprite();

            RedCircle.Texture = GD.Load<Texture>("res://Pruebas/RedCircle.png");

            RedCircle.ShowBehindParent = true;

            enemieTutorial.AddChild(RedCircle);

            enemieTutorial.Start(5, new Navigation2D(), new Vector2(1718, 320), 100);

            MobGenerator.GetNode<Node2D>("Enemies").AddChild(enemieTutorial);

            enemieTutorial.Connect("EnemieDeath", this, "FifthTutorial", new Godot.Collections.Array(new Node[]{fourthTutorial, tutorialAutoShoot}));

            //tutorialAutoShoot.GetNode<Area2D>("Area2D").Connect("input_event", this, "FifthTutorial", new Godot.Collections.Array(new Node[]{fourthTutorial, tutorialAutoShoot}));

            Tutorials.CallDeferred("add_child", tutorialAutoShoot);

        }

    }

    public void FifthTutorial(TutorialText removeTutorial, Node2D removeTutorial2){

        removeTutorial.QueueFree();

        removeTutorial2.CallDeferred("queue_free");

        var fifthTutorial =  InitTutorialText(4);

        GameData.GetNode<Label>("Coins").Visible = true;

        Area2D tutorialWalk = (GD.Load("res://World/Maps/Tutorial/TutorialWalk.tscn") as PackedScene).Instance() as Area2D;

        TutorialTerrain.CallDeferred("add_child", tutorialWalk);

        tutorialWalk.Position = new Vector2(1728, 928);

        tutorialWalk.RotationDegrees = 90;

        Godot.Collections.Array sendData = new Godot.Collections.Array();

        sendData.Add(fifthTutorial);

        sendData.Add(tutorialWalk);

        tutorialWalk.Connect("body_entered", this, "SixthTutorial", sendData);

    }

    public void SixthTutorial(Node2D body, TutorialText removeTutorial, Area2D removeArea){

        removeArea.Disconnect("body_entered", this, "SixthTutorial");

        removeTutorial.QueueFree();

        removeArea.CallDeferred("queue_free");

        var sixthTutorial =  InitTutorialText(5);
        GD.Print(body.Name);
        Player.Connect("ChangeWeapons", this, "SeventhTutorial", new Godot.Collections.Array(new TutorialText[]{sixthTutorial}));

    }

    public void SeventhTutorial(TutorialText removeTutorial){

        removeTutorial.QueueFree();

        Controls.GetNode<Node2D>("Weapons").Visible = true;

        GameData.GetNode<Label>("WeaponInfo").Visible = true;

        var seventhTutorial =  InitTutorialText(6);

        Area2D tutorialWalk = (GD.Load("res://World/Maps/Tutorial/TutorialWalk.tscn") as PackedScene).Instance() as Area2D;

        TutorialTerrain.CallDeferred("add_child", tutorialWalk);

        tutorialWalk.Position = new Vector2(1088, 1226);

        Godot.Collections.Array sendData = new Godot.Collections.Array();

        sendData.Add(seventhTutorial);

        sendData.Add(tutorialWalk);

        tutorialWalk.Connect("body_entered", this, "EighthTutorial", sendData);

    }

    public async void EighthTutorial(Node2D body, TutorialText removeTutorial, Area2D removeArea){

        removeTutorial.QueueFree();

        removeArea.CallDeferred("queue_free");

        var eighthTutorial =  InitTutorialText(7);

        foreach(CanvasItem i in GameData.GetChildren()){

            i.Visible = true;

        }

        StaticBody2D obstacle = GD.Load<PackedScene>("res://World/Maps/Tutorial/Obstacle.tscn").Instance() as StaticBody2D;

        TutorialTerrain.CallDeferred("add_child", obstacle);

        await ToSignal(GetTree().CreateTimer(20), "timeout");

        eighthTutorial.QueueFree();

        StartZombies();

    }

    public void StartZombies(){

        MobGenerator.Start(GetNode<Navigation2D>("NavigableTerrain"));

        MobGenerator.Connect("ChangeZombies", this, "Finished");

        GameData.SetRound("Tutorial Round");

    }

    public async void Finished(int remaining, int total){

        GD.Print(remaining);

        if(remaining <= 0){

            var finished =  InitTutorialText(8);

            await ToSignal(GetTree().CreateTimer(10), "timeout");

            GetTree().ChangeScene("res://World/Maps/Map1/Map1.tscn");


        }

        

    }



}

