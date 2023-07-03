using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSet
{
    private int rotation;
    public List<Tile> Tiles { get; set; }
    public List<Sprite> Sprites { get; set; }

    public TileSet(List<Sprite> sprites)
    {
        Tiles = new List<Tile>(16);
        Sprites = sprites;

        for (int i = 0; i < 16; i++)
        {
            Tiles.Add(new Tile());
        }

        for (int i = 0; i < Tiles.Capacity; i++)
        {
            rotation = ((i+1) % 4) * 90;
            if (i == 0)
            {
                Tiles[i] = new Tile( Socket.empty, Socket.empty, Socket.empty, Socket.empty, i, sprites[i]);
            }
            else if (i == 1)
            {
                Tiles[i] = RotateTile(BaseTiles.Tile1, 0, i);
            }
            else if (i == 2)
            {
                Tiles[i] = RotateTile(BaseTiles.Tile1, 90, i);
            }
            else if (i > 2 && i < 7)
            {                    
                Tiles[i] = RotateTile(BaseTiles.Tile2, rotation, i);
            }
            else if (i > 6 && i < 11)
            {
                Tiles[i] = RotateTile(BaseTiles.Tile3, rotation, i);
            }
            else if (i > 10 && i < 15)
            {
                Tiles[i] = RotateTile(BaseTiles.Tile4, rotation, i);
            }
            else if (i == 15)
            { 
                Tiles[i] = new Tile( Socket.line, Socket.line, Socket.line, Socket.line, i, sprites[i]);
            }
        }
    } 
    Tile RotateTile(Tile tile, int rotation, int index)
    {
        Tile rotatedList = new Tile();
        List<Socket> sockets = new List<Socket>(4);

        if (rotation == 0)
        {
            sockets = new List<Socket>() { tile.NORTH, tile.EAST, tile.SOUTH, tile.WEST };
            rotatedList =  new Tile(sockets[0], sockets[1], sockets[2], sockets[3], index, Sprites[index]);
        }
        else if (rotation == 90)
        {
            sockets = new List<Socket>() { tile.WEST, tile.NORTH, tile.EAST, tile.SOUTH };
            rotatedList =  new Tile(sockets[0], sockets[1], sockets[2], sockets[3], index, Sprites[index]);
        }
        else if (rotation == 180)
        {
            sockets = new List<Socket>(){ tile.SOUTH, tile.WEST, tile.NORTH, tile.EAST };            
            rotatedList =  new Tile(sockets[0], sockets[1], sockets[2], sockets[3], index, Sprites[index]);
        }
        else if (rotation == 270)
        {            
            sockets = new List<Socket>() { tile.EAST, tile.SOUTH, tile.WEST, tile.NORTH };  
            rotatedList =  new Tile(sockets[0], sockets[1], sockets[2], sockets[3], index, Sprites[index]);
        }

        return rotatedList;
    }
}
