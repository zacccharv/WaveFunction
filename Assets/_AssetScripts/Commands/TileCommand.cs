public abstract class TileCommand : Command
{
    public abstract CellBase CellBase { get; set; }
    public abstract CommandManager CommandManager { get; set; }
    public abstract GridManager GridManager { get; set; }
}