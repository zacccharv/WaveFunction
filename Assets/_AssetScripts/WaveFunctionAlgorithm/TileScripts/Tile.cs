using System.Collections;
public struct Tile
{
    public Tile(Socket north, Socket east, Socket south, Socket west)
    {
        NORTH = north;
        EAST = east;
        SOUTH = south;
        WEST = west;
    }
    public Socket NORTH { get; }
    public Socket EAST { get; }
    public Socket SOUTH { get; }
    public Socket WEST { get; }

    public override string ToString() => $"({NORTH}, {EAST}, {SOUTH}, {WEST})";
}
