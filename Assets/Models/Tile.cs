using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile
{
    public enum TileType { Empty, Grass, Stone, Water };
    TileType type = TileType.Empty;

    StaticObject staticObject;
    DynamicObject dynamicObject;

    World world;
    int x;
    int y;

    Action<Tile> tileTypeChanged;

    // ========================================================== //

    public Tile(World world, int x, int y)
    {
        this.world = world;
        this.x = x;
        this.y = y;
    }

    // ========================================================== //



    // ========================================================== //

    public TileType Type {
        get {
            return type;
        }
        set {
            TileType oldType = type;
            type = value;

            if (tileTypeChanged != null && oldType != type) {
                tileTypeChanged(this);
            }
        }
    }

    public int X {
        get {
            return x;
        }
    }

    public int Y {
        get {
            return y;
        }
    }

    public void RegisterTileTypeCallback(Action<Tile> callback)
    {
        tileTypeChanged += callback;
    }

    public void UnregisterTileTypeCallback(Action<Tile> callback)
    {
        tileTypeChanged -= callback;
    }


}




