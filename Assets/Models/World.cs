using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    Tile[,] tiles;
    int width;
    int height;

    public World(int width = 300, int height = 300)
    {
        this.width = width;
        this.height = height;

        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                
                tiles[x,y] = new Tile(this, x, y);
            }
        }
    }

    public void RandomizeTiles()
    {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                
                if (Random.Range(0, 2) == 0) {
                    tiles[x,y].Type = Tile.TileType.Empty;
                } else {
                    tiles[x,y].Type = Tile.TileType.Grass;
                }
            }
        }
    }

    // ========================================================== //

    public Tile GetTileAt(int x, int y)
    {
        if (x >= Width || x < 0 || y >= Height || y < 0) {
            return null;
        }
        return tiles[x, y];
    }

    public int Width {
        get {
            return width;
        }
    }

    public int Height {
        get {
            return height;
        }
    }

}


