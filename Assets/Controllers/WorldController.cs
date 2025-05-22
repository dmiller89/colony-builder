using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldController : MonoBehaviour
{
    public static WorldController Instance { get; protected set; }
    public World World { get; protected set; }
    
    public Sprite grassSprite;
    public Sprite stoneSprite;
    public Sprite waterSprite;

    void Start()
    {
        SetWorldInstance();

        // (1) create object for each tile
        // (2) apply noise to the world and change tile types accordingly

        // (1)
        for (int x = 0; x < World.Width; x++) {
            for (int y = 0; y < World.Height; y++) {

                GameObject tileObject = new GameObject();
                tileObject.name = "Tile (" + x + ", " + y + ")";
                Tile tileData = World.GetTileAt(x, y);

                tileObject.transform.position = new Vector3(tileData.X, tileData.Y, 0f);
                tileObject.transform.SetParent(this.transform, true);

                tileObject.AddComponent<SpriteRenderer>();

                tileData.RegisterTileTypeCallback(
                    (tile) => { OnTileTypeChanged(tile, tileObject); }
                );
            }
        }

        World.RandomizeTiles();
    }

    void SetWorldInstance()
    {
        if (Instance != null) {
            Debug.LogError("There should never be more than one WorldController");
        }
        Instance = this;

        World = new World();
    }

    void OnTileTypeChanged(Tile tileData, GameObject tileObject)
    {
        if (tileData.Type == Tile.TileType.Empty) {
            tileObject.GetComponent<SpriteRenderer>().sprite = null;
        }

        else if (tileData.Type == Tile.TileType.Grass) {
            tileObject.GetComponent<SpriteRenderer>().sprite = grassSprite;
        }
        
        else if (tileData.Type == Tile.TileType.Stone) {
            tileObject.GetComponent<SpriteRenderer>().sprite = stoneSprite;
        }
        
        else if (tileData.Type == Tile.TileType.Water) {
            tileObject.GetComponent<SpriteRenderer>().sprite = waterSprite;
        }

        else {
            Debug.LogError("Unrecognized Tile type");
        }
    }

    public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        return World.GetTileAt(x, y);
    }


}


