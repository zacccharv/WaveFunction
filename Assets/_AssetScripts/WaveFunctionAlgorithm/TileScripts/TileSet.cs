using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSet
{
    private int rotation;
    public List<Tile> Tiles { get; set; }
    public TileSet()
    {
        Tiles = new List<Tile>(16);

        for (int i = 0; i < 16; i++)
        {
            Tiles.Add(new Tile());
        }

        for (int i = 0; i < Tiles.Capacity; i++)
        {
            rotation = ((i+1) % 4) * 90;
            if (i == 0)
            {
                Tiles[i] = BaseTiles.Tile0;
            }
            else if (i == 1)
            {
                Tiles[i] = RotateTile(BaseTiles.Tile1, 0);
            }
            else if (i == 2)
            {
                Tiles[i] = RotateTile(BaseTiles.Tile1, 90);
            }
            else if (i > 2 && i < 7)
            {                    
                Tiles[i] = RotateTile(BaseTiles.Tile2, rotation);
            }
            else if (i > 6 && i < 11)
            {
                Tiles[i] = RotateTile(BaseTiles.Tile3, rotation);
            }
            else if (i > 10 && i < 15)
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
        List<Socket> sockets = new List<Socket>(4);

        if (rotation == 0)
        {
            rotatedList = tile;
        }
        else if (rotation == 90)
        {
            sockets = new List<Socket>() { tile.WEST, tile.NORTH, tile.EAST, tile.SOUTH };
            rotatedList =  new Tile(sockets[0], sockets[1], sockets[2], sockets[3]);
        }
        else if (rotation == 180)
        {
            sockets = new List<Socket>(){ tile.SOUTH, tile.WEST, tile.NORTH, tile.EAST };            
            rotatedList =  new Tile(sockets[0], sockets[1], sockets[2], sockets[3]);
        }
        else if (rotation == 270)
        {            
            sockets = new List<Socket>() { tile.EAST, tile.SOUTH, tile.WEST, tile.NORTH };  
            rotatedList =  new Tile(sockets[0], sockets[1], sockets[2], sockets[3]);
        }

        return rotatedList;
    }
}
