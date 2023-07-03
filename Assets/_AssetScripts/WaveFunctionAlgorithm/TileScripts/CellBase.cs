using System.Collections;
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
    public List<CellBase> NeighborCells = new List<CellBase>();
    
    public bool startTile;
    public List<Socket> sockets = new List<Socket>();
    public List<string> tileStrings = new List<string>();
    public Sprite initSprite;
    public bool failed;
    public SpriteRenderer spriteRenderer;

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
            CollapseCommand collapse = new CollapseCommand(this, CommandManager, GridManager);
            collapse.Execute();

            yield return null;

            EleminateCommand eleminate = new EleminateCommand(this, CommandManager, GridManager);
            eleminate.Execute();

            yield return null;

            // WestToEastWave();

            // SpiralWave();

            Cell.EntropyWave(NeighborCells);
        }

        if (this.sockets.Count == 0)
        {
            Debug.Log($"Uncollapsed at Index {Position} but shouldn't have been. After Entropy");
        }

        Collapsed = true;
        

        CreateTileStrings(Cell.tileSet.Tiles);        
        yield return null;

        // SetTileStrings(tileStrings);
        // yield return null;
    }
    public void CreateTileStrings(List<Tile> tiles)
    {
        foreach (var item in tiles)
        {
            tileStrings.Add(item.ToString());
        }
    }
    public void SetTileStrings(List<string> tiles)
    {
        List<string> m = new List<string>();

        foreach (var item in tiles)
        {
            m.Add(item);
        }

        tileStrings.Clear();

        m.Reverse();

        foreach (var item in m)
        {
            tileStrings.Add(item);
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