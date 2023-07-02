using System.Collections.Generic;
using UnityEngine;

public class CellBase : MonoBehaviour, ICell
{
    public event System.Action tileCollapseEvent;

    [field: SerializeField] public Cell Cell { get; set; }
    public GridManager GridManager { get { return FindAnyObjectByType<GridManager>(); } }
    public CommandManager CommandManager { get { return FindAnyObjectByType<CommandManager>(); } }
    public WaveFunctionManager WaveFunctionManager { get { return FindAnyObjectByType<WaveFunctionManager>(); } }
    
    [field: SerializeField] public Vector2 Position { get; set; } = new Vector2(0, 0);
    [field: SerializeField] public int Index { get; set; } = 0;
    [field: SerializeField] public bool Collapsed { get; set; }
    public List<Cell> NeighborCells = new List<Cell>();
    
    public bool startTile;
    public List<Socket> sockets = new List<Socket>();
    public List<string> tileStrings = new List<string>();
    public Sprite initSprite;
    public bool failed;
    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        Cell = new Cell(this);
        spriteRenderer = GetComponent<SpriteRenderer>();

        for (var i = 0; i < 4; i++)
        {
            NeighborCells.Add(new Cell(this));
        }             
    }
    void OnEnable()
    {
        tileCollapseEvent += Cell.RunCoroutine;
    }
    void OnDisable()
    {
        tileCollapseEvent -= Cell.RunCoroutine;
    }
    public void OnCollapsed()
    {
        tileCollapseEvent?.Invoke();
    }
    public void SetTileStrings(List<Tile> tiles)
    {
        tileStrings.Clear();

        foreach (var item in tiles)
        {
            tileStrings.Add(item.ToString());
        }
    }
}

internal interface ICell
{
    public GridManager GridManager { get; }
    public CommandManager CommandManager { get; }
    public WaveFunctionManager WaveFunctionManager { get; }
    public Vector2 Position { get; set; }
    public int Index { get; set; }
    public bool Collapsed { get; set; }
    public void OnCollapsed() {} 
}