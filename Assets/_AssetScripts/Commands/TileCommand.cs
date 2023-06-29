public abstract class TileCommand : Command
{
    public abstract CurrentTile CurrentTile { get; set; }
    public abstract CommandManager CommandManager { get; set; }
    public abstract GridManager GridManager { get; set; }
}