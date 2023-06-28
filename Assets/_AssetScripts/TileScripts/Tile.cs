using System.Collections;
public struct Tile
{
    public Tile(Socket a = Socket.empty, Socket b = Socket.empty, Socket c = Socket.empty, Socket d = Socket.empty)
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
