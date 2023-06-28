public static class BaseTiles
{
    public static Tile Tile0 {get;} = new Tile(Socket.empty, Socket.empty, Socket.empty, Socket.empty) ;
    public static Tile Tile1 {get;} = new Tile(Socket.empty, Socket.line, Socket.empty, Socket.line);
    public static Tile Tile2 {get;} = new Tile(Socket.empty, Socket.empty, Socket.empty, Socket.line);
    public static Tile Tile3 {get;} = new Tile(Socket.line, Socket.line, Socket.empty, Socket.empty);
    public static Tile Tile4 {get;} = new Tile(Socket.line, Socket.line, Socket.empty, Socket.line);    
    public static Tile Tile5 {get;} = new Tile(Socket.line, Socket.line, Socket.line, Socket.line);     
}
