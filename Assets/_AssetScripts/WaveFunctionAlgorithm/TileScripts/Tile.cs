using System.Collections;
using UnityEngine;

public struct Tile
{
    public Tile(Socket north, Socket east, Socket south, Socket west, int listIndex = 0, Sprite sprite = null)
    {
        NORTH = north;
        EAST = east;
        SOUTH = south;
        WEST = west;
        LISTINDEX = listIndex;
        SPRITE = sprite;
    }
    public Socket NORTH { get; }
    public Socket EAST { get; }
    public Socket SOUTH { get; }
    public Socket WEST { get; }
    public int LISTINDEX { get; }
    public Sprite SPRITE { get; }

    public override string ToString() => $"({NORTH}, {EAST}, {SOUTH}, {WEST}, {LISTINDEX})";
}
