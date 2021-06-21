using Godot;
using System;


public class MiniMap : TextureRect
{
    
    
    [Export] NodePath cameraPath;
    [Export] NodePath tilemapPath;
    [Export] NodePath Player;
    [Export] float zoom = 1.5f;

    Camera2D camera;
    TileMap TileMap;
    Godot.Collections.Dictionary<String, Sprite> icons = new Godot.Collections.Dictionary<String, Sprite>();
    Godot.Collections.Dictionary<Node2D, Sprite> markers = new Godot.Collections.Dictionary<Node2D, Sprite>();
    Godot.Collections.Array markersKeys = new Godot.Collections.Array();
    Vector2 gridScale = new Vector2(128, 128);
    public override void _Ready()
    {
        
        camera = GetNode<Camera2D>(cameraPath);

        TileMap = GetNode<TileMap>(tilemapPath);

        icons.Add("enemie", GetNode<Sprite>("EnemieMarker"));

        gridScale = gridScale / (GetViewportRect().Size * zoom);

        Godot.Collections.Array mapObjects = GetTree().GetNodesInGroup("MiniMapObjects");

        foreach(Node2D i in mapObjects){

            NewMarker(i);
            
        }


    }

    public void NewMarker(Node2D marker){

        Sprite newMarker = icons[marker.Get("miniMap").ToString()].Duplicate() as Sprite;

        AddChild(newMarker);

        newMarker.Show();

        markers.Add(marker, newMarker);

        markersKeys.Add(marker);
    }

    public void RemoveMarker(Node2D marker){

        markers[marker].QueueFree();

        markers.Remove(marker);

        markersKeys.Remove(marker);

    }


    public override void _Process(float delta){

        foreach(Node2D i in markersKeys){

            Vector2 objPos = (i.Position - GetNode<Player>(Player).Position) * gridScale + new Vector2(64, 64) ;

            objPos.x = Mathf.Clamp(objPos.x, 0, 128);

            objPos.y = Mathf.Clamp(objPos.y, 0, 128);

            markers[i].Position = objPos;

        }

        Update();

    }

    public Godot.Collections.Array GetCells(){

        return TileMap.GetUsedCells();

    }

    public override void _Draw()
    {
        DrawSetTransform(RectSize / 2, 0, new Vector2(zoom, zoom));

        Vector2 cameraPosition = camera.GetCameraScreenCenter();

        Vector2 cameraCell = TileMap.WorldToMap(cameraPosition);

        Vector2 tilemapOffset = cameraCell + (cameraPosition - TileMap.MapToWorld(cameraCell)) / TileMap.CellSize;

        var cells = GetCells();

        Rect2 rectangle = new Rect2(-40, -40, 80, 80);

        foreach(Vector2 i in cells){

            if(rectangle.HasPoint((i - tilemapOffset) * zoom)){

                 DrawRect(new Rect2((i - tilemapOffset) * zoom, Vector2.One * zoom), new Color(0, 0, 0, 1));


            }

           
        }


    }

}
