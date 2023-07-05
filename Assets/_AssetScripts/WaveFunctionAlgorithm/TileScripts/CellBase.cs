using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBase : MonoBehaviour, ICell
{
    public event System.Action tileCollapseEvent;

    public CellBase previousCellBase = null;
    [field: SerializeField] public Cell Cell { get; set; }
    public GridManager GridManager { get { return FindAnyObjectByType<GridManager>(); } }
    public CommandManager CommandManager { get { return FindAnyObjectByType<CommandManager>(); } }
    public WaveFunctionManager WaveFunctionManager { get { return FindAnyObjectByType<WaveFunctionManager>(); } }
    
    [field: SerializeField] public Vector2 Position { get; set; } = new Vector2(0, 0);
    [field: SerializeField] public int Index { get; set; } = 0;
    [field: SerializeField] public bool Collapsed { get; set; }
    public List<CellBase> NeighborCells = new List<CellBase>();
    public List<CellBase> OriginalNeighbors = new List<CellBase>();

    public List<Socket> sockets = new List<Socket>();
    public Sprite initSprite;
    public SpriteRenderer spriteRenderer;

    public List<string> tileStrings = new List<string>();
    public bool startTile;
    
    public List<bool> allowedTiles = new List<bool>() { false, false, false, false };
    private bool failed;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initSprite = spriteRenderer.sprite;
        
        Cell = new Cell(this);
        
        for (var i = 0; i < 4; i++)
        {
            NeighborCells.Add(this);
        }

        Cell.SetNeighbors();
        CreateTileStrings(Cell.tileSet.Tiles);

        if (Index == 1)
        {
            RunCoroutine();
        }
    }
    void OnEnable()
    {
        tileCollapseEvent += RunCoroutine;
    }
    void OnDisable()
    {
        tileCollapseEvent -= RunCoroutine;
    }
    public void RunCoroutine()
    {
        StartCoroutine(WaveFunction());
    }
    public void OnCollapsed()
    {
        tileCollapseEvent?.Invoke();
    }
    
    public IEnumerator WaveFunction()
    {
        if (GridManager.waveIndex.Count > (GridManager.DIM * GridManager.DIM) - 1)
        {
            Debug.Log("WaveFunction Cancelled");
            StopAllCoroutines();
        } 

        if (!Collapsed)
        {
            OriginalNeighbors = NeighborCells;

            CollapseCommand collapse = new CollapseCommand(this, CommandManager, GridManager);
            collapse.Execute();
            yield return null;


            EleminateCommand eleminate = new EleminateCommand(this, CommandManager, GridManager);
            eleminate.Execute();

            yield return null;

            // WestToEastWave();

            // SpiralWave();

            Cell nextTile = Cell.EntropyWave(NeighborCells);
            
            CreateTileStrings(this.Cell.tileSet.Tiles);

            if (nextTile != null)
            {
                if (failed)
                {
                    Cell.SetNeighbors();
                    
                    List<int> neighborTileCount = new List<int>() { };
                    neighborTileCount.Capacity = NeighborCells.Count;

                    foreach (var item in NeighborCells)
                    {
                        if (item != null)
                        {
                            neighborTileCount.Add(item.Cell.tileSet.Tiles.Count);
                        }
                    }

                    string str1 = string.Join(", neighbor tile number = ", neighborTileCount);
                    Debug.Log($"{str1} + Index = {Index}");
                }

                nextTile.CellBase.previousCellBase = this;
                nextTile.CellBase.OnCollapsed();
            }
            else if (nextTile == null)
            {
                // Debug.Log($"this Cell = {this.Cell.ToString()}, previous Cell = {previousCellBase.Cell.ToString()} previous Collapsed = {previousCellBase.Cell.Collapsed}");

                // Debug.Log(string.Join(", ", Cell.tileSet.Tiles));

                // for (var i = 0; i < 4; i++)
                // {
                //     CommandManager.UndoLastTileCommand();
                //     Debug.Log();
                //     yield return null;
                // }

                Undo(OriginalNeighbors);
                previousCellBase.Undo(previousCellBase.OriginalNeighbors);

                previousCellBase.failed = true;
                failed = false;

                previousCellBase.allowedTiles[Cell.failedIndex] = false;
                //previousCellBase.Cell.tileSet.Tiles.RemoveAt(Cell.currentTileIndex);

                previousCellBase.Collapsed = false;
                previousCellBase.OnCollapsed();
                yield return null;
            }

            yield return null;
        }

        Collapsed = true;
        yield return null;        
    }
    void Undo(List<CellBase> _neighbors)
    {        
        spriteRenderer.sprite = initSprite;
        Cell.tile = new Tile();
        sockets = new List<Socket>();

        List<string> neighborCells = new List<string>() { "", "", "", ""};

        for (int i = 0; i < neighborCells.Count; i++)
        {
            if (NeighborCells[i] != null)
            {
                neighborCells[i] = $"NeighborCells[{i}]";
            }
        }

        for (var i = 0; i < NeighborCells.Count; i++)
        {
            if (neighborCells[i] != "")
            {
                NeighborCells[i].Cell.tileSet.Tiles = _neighbors[i].Cell.tileSet.Tiles;
                NeighborCells[i].CreateTileStrings(NeighborCells[i].Cell.tileSet.Tiles); 
            }
        }
    }
    public void CreateTileStrings(List<Tile> tiles)
    {
        tileStrings.Clear();

        foreach (var item in tiles)
        {   
            if (tiles.Count < 17)
            {
               tileStrings.Add(item.ToString()); 
            }
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
}