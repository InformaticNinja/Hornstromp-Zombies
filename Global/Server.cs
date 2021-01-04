using Godot;
using System;

public class Server : Node
{
    
    NetworkedMultiplayerENet Network = new NetworkedMultiplayerENet();
    String ip = "127.0.0.1";
    int port = 1909;
        public override void _Ready(){
        
        ConnectToServer();

    }


    public void ConnectToServer(){

        Network.CreateClient(ip, port);

        GetTree().NetworkPeer = Network;

        Network.Connect("connection_failed", this, "_OnConnectionFailed");

        Network.Connect("connection_succeeded", this, "_OnConnectionSucceeded");

    }

    public void _OnConnectionFailed(){


    }

    public void _OnConnectionSucceeded(){


    }

}
