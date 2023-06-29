using System.Collections;
public struct Tile
{
    public Tile(Socket a, Socket b, Socket c, Socket d)
    {
        NORTH = a;
        EAST = b;
        SOUTH = c;
        WEST = d;
    }
    public Socket NORTH { get; }
    public Socket EAST { get; }
    public Socket SOUTH { get; }
    public Socket WEST { get; }

    public override string ToString() => $"({NORTH}, {EAST}, {SOUTH}, {WEST})";
}
