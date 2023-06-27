using System.Collections.Generic;
using UnityEngine;

public class TileSet
{
    private int rotation;
    public List<Tile> Tiles { get; private set; }
    public TileSet()
    {
        Tiles = new List<Tile>(16);

        for (int i = 0; i < 16; i++)
        {
            Tiles.Add(new Tile());
        }

        for (int i = 0; i < Tiles.Capacity - 1; i++)
        {
            rotation = ((i-1) % 4) * 90;

            if (i == 0)
            {
                Tiles[i] = BaseTiles.Tile0;
            }
            else if (i >= 1 && i <= 2)
            {
                Tiles[i] = RotateTile(BaseTiles.Tile1, rotation);
            }
            else if (i >= 3 && i <= 6)
            {                    
                Tiles[i] = RotateTile(BaseTiles.Tile2, rotation);
            }
            else if (i >= 7 && i <= 10)
            {
                Tiles[i] = RotateTile(BaseTiles.Tile3, rotation);
            }
            else if (i >= 11 && i <= 14)
            {
                Tiles[i] = RotateTile(BaseTiles.Tile4, rotation);
            }
            else if (i == 15)
            { 
                Tiles[i] = BaseTiles.Tile5;
            }
        }
    } 
    Tile RotateTile(Tile tile, int rotation)
    {
        Tile rotatedList = new Tile();

        if (rotation == 0)
        {
            rotatedList = tile;
        }
        else if (rotation == 90)
        {
            rotatedList =  new Tile(tile.SOUTH, tile.NORTH, tile.EAST, tile.WEST);
        }
        else if (rotation == 180)
        {
            rotatedList = new Tile(tile.WEST, tile.SOUTH, tile.NORTH, tile.EAST);
        }
        else if (rotation == 270)
        {
            rotatedList = new Tile(tile.EAST, tile.WEST, tile.SOUTH, tile.NORTH);
        }

        return rotatedList;
    }
}
