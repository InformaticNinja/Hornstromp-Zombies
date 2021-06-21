using Godot;
using System;

public class Global : Node
{

    Godot.Collections.Dictionary<String, String> PATHS = new Godot.Collections.Dictionary<String, String>();
    String PASS = "gafglmcvale1ga2v4a151basGA5212";
       public override void _Ready(){
        
           StartPaths();



    }

    public void StartPaths(){

        PATHS.Add("Data", "user://data.dat");

        PATHS.Add("Settings", "user://settings.dat");

    }

    public object LoadFileVar(String file, object defaultValue){

        File loadFile= new File();

        if(loadFile.FileExists(file)){

            loadFile.OpenEncryptedWithPass(file, Godot.File.ModeFlags.Read, PASS);

            defaultValue = loadFile.GetVar() as object;

        }else{

            SaveFile(file, defaultValue);

        }

        loadFile.Close();

        return defaultValue;

    }

    public void LoadFile(String file, Godot.Collections.Dictionary loadData = null){

        File loadFile = new File();

        if(loadData == null){
            loadData = Get(file) as Godot.Collections.Dictionary;
            file = PATHS[file];
            
        }

        if(loadFile.FileExists(file)){

            loadFile.OpenEncryptedWithPass(file, Godot.File.ModeFlags.Read, PASS);

            Godot.Collections.Dictionary getVar = loadFile.GetVar() as Godot.Collections.Dictionary;

            Godot.Collections.Array keys = new Godot.Collections.Array(getVar.Keys);

            foreach(String i in keys){

                loadData[i] = getVar[i];

            }

        }else{

            SaveFile(file, loadData);

        }

        loadFile.Close();

    }

    public void SaveFile(String file, object saveData = null){

        File saveFile = new File();

        if(saveData == null){
            file = PATHS[file];
        }

        saveFile.OpenEncryptedWithPass(file, File.ModeFlags.Write, PASS);

        saveFile.StoreVar(saveData);

        saveFile.Close();

    }

    public void ConnectButtons(){

        Godot.Collections.Array buttons = GetTree().GetNodesInGroup("buttons");

        foreach(TextureButton button in buttons){

            if(!IsInGroup("connected")){

                Godot.Collections.Array buttonConnected = new Godot.Collections.Array();

                buttonConnected.Add(button);

                button.Connect("button_down", this, "ButtonDown", buttonConnected);

                button.Connect("button_up", this, "ButtonUp", buttonConnected);

                button.AddToGroup("connected");

            }

        }

    }

    
    public void ButtonDown(TextureButton button){

        button.RectScale = new Vector2(.9f, .9f);


    }

    public void ButtonUP(TextureButton button){

        button.RectScale = new Vector2(1, 1);

    }

    public void ButtonDisabled(TextureButton button, bool disabled){

        button.Disabled = disabled;

        if(disabled)

        button.Modulate = new Color(.3f, .3f, .3f, 1f);

        else

        button.Modulate = new Color(1, 1, 1, 1);
        

    }



}
