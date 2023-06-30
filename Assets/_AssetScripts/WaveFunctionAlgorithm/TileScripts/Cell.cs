using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Cell : CellBase
{
    public bool startTile;
    public bool failed;
    public Tile tile = new Tile();
    public TileSet tileSet = new TileSet();
    public List<Socket> sockets = new List<Socket>();
    public Sprite initSprite;
    public Cell northNeighbor;
    public Cell eastNeighbor;
    public Cell southNeighbor;
    public Cell westNeighbor;    

    public List<Sprite> backgrounds = new List<Sprite>();
    public SpriteRenderer spriteRenderer;

    int NE = 1; int SE = 2; int SW = 3; int NW = 4;
    private void OnEnable() 
    {
        tileCollapseEvent += RunCoroutine;
    }
    private void OnDisable() 
    {
        tileCollapseEvent -= RunCoroutine;
    }

    private void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetNeighbors();

        if (startTile)
        {
            StartCoroutine(WaveFunction());
        }
    }
    private void RunCoroutine()
    {
        StartCoroutine(WaveFunction());
    }

    public IEnumerator WaveFunction()
    {
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

            EntropyWave();

            yield return null;
        }

        Collapsed = true;
        yield return null;

    }
    void EntropyWave()
    {
        List<int> entropyAmounts = new List<int>();

        void GetEntropy(Cell north = null, Cell east = null, Cell south = null, Cell west = null)
        {
            Cell[] tileSet = { north, east, south, west };

            for (int i = 0; i < tileSet.Length; i++)
            {
               if (tileSet[i] != null && !tileSet[i].Collapsed) entropyAmounts.Add(tileSet[i].tileSet.Tiles.Count);
            }
        }
        GetEntropy(southNeighbor, eastNeighbor, northNeighbor, westNeighbor);
        
        int smallestEntropy = Mathf.Min(entropyAmounts.ToArray());
        Debug.Log(string.Join(", ", entropyAmounts.ToArray()));

        Cell CollapsibleTile(Cell north = null, Cell east = null, Cell south = null, Cell west = null)
        {
            Cell[] tileSet = { north, east, south, west };

            for (int i = 0; i < tileSet.Length; i++)
            {
                if (tileSet[i] != null && tileSet[i].tileSet.Tiles.Count == smallestEntropy && !tileSet[i].Collapsed)
                {
                    return tileSet[i];
                }
            }
            StopAllCoroutines();
            return null;
        }

        CollapsibleTile(northNeighbor, eastNeighbor, southNeighbor, westNeighbor).OnCollapsed();
    }
    void SpiralWave()
        {
            WaveFunctionManager.waveDirection = tileDirection();

            if (tileDirection() == Direction.NORTH)
            {
                northNeighbor.OnCollapsed();
            }
            else if (tileDirection() == Direction.EAST)
            {
                eastNeighbor.OnCollapsed();
            }
            else if (tileDirection() == Direction.SOUTH)
            {
                southNeighbor.OnCollapsed();
            } 
            else if(tileDirection() == Direction.WEST)
            {
                westNeighbor.OnCollapsed();
            }
        }
    void WestToEastWave()
    {
        if (!GridManager.grid[Index].Collapsed)
        {
            GridManager.grid[Index].OnCollapsed();
        }
    }
    Direction tileDirection()
    {
        // North West or West        
        if ((GetCornerContacts() == SW ^ GetContacts()[3]) && !GetContacts()[0]) return Direction.NORTH;

        // North East or North
        else if ((GetCornerContacts() == NW ^ GetContacts()[0]) && !GetContacts()[1] ) return Direction.EAST;
        
        // North East or South
        else if ((GetCornerContacts() == NW ^ GetContacts()[1]) && !GetContacts()[2] ) return Direction.SOUTH;

        // North East or South
        else if ((GetCornerContacts() == SE ^ GetContacts()[2]) && !GetContacts()[3] ) return Direction.WEST;
        Debug.Log(string.Join(", ", GetContacts().ToArray()));

        return Direction.NONE;
    }
    List<bool> GetContacts()
    {
        bool northCollapse = northNeighbor == null || northNeighbor.Collapsed;
        bool eastCollapse = eastNeighbor == null || eastNeighbor.Collapsed;
        bool southCollapse = southNeighbor == null || southNeighbor.Collapsed;
        bool westCollapse = westNeighbor == null || westNeighbor.Collapsed;

        return new List<bool>() { northCollapse, eastCollapse, southCollapse, westCollapse };
    }
    int GetCornerContacts()
    {
        if (GetContacts() == new List<bool>(){true, true, false, false})
        {
            return NE;
        }
        else if (GetContacts() == new List<bool>(){false, true, true, false})
        {
            return SE;
        }
        else if (GetContacts() == new List<bool>(){false, false, true, true})
        {
            return SW;
        }
        else if (GetContacts() == new List<bool>(){true, false, false, true})
        {
            return NW;
        }
        return 0;
    }
    void SetNeighbors()
    {
        List<Cell> grid = GridManager.grid;
        bool NorthNeighborExists()
        {
            return !(Mathf.Ceil(Index / GridManager.columnNumber) == GridManager.columnNumber - 1 || Index == GridManager.columnNumber * GridManager.rowNumber);
        }
        bool EastNeighborExists()
        {
            return !(Index % GridManager.columnNumber == 0);
        }
        bool SouthNeighborExists()
        {
            return !(Mathf.Ceil(Index / GridManager.columnNumber) == 0 || Index == GridManager.columnNumber);
        }        
        bool WestNeighborExists()
        {
            return !(Index % GridManager.columnNumber == 1);
        }

        if (NorthNeighborExists())
        {
            northNeighbor = (grid[(Index - 1) + GridManager.columnNumber]);
        }
        if (EastNeighborExists())
        {
            eastNeighbor = (grid[(Index - 1) + 1]);            
        }
        if (SouthNeighborExists())
        {
            southNeighbor = (southNeighbor = grid[(Index - 1) - GridManager.columnNumber]);            
        }
        if (WestNeighborExists())
        {
            westNeighbor = (westNeighbor = grid[(Index - 1) - 1]);
        }   
    }  
    public void RollForTile(int random)
    {
        spriteRenderer.sprite = backgrounds[random];
        tile = tileSet.Tiles[random];

        sockets = new List<Socket>() { tile.NORTH, tile.EAST, tile.SOUTH, tile.WEST };
        return;
    }
    public void RemoveNeighborsFromLists()
    {
        if (northNeighbor != null) 
        {
            List<Tile> neighbors = new List<Tile>();
            List<Sprite> sprites = new List<Sprite>();

            for (int i = 0; i < northNeighbor.tileSet.Tiles.Count; i++)
            {   
                if (tile.NORTH == northNeighbor.tileSet.Tiles[i].SOUTH)
                {
                    neighbors.Add(northNeighbor.tileSet.Tiles[i]);
                    sprites.Add(northNeighbor.backgrounds[i]);
                }
            }
            northNeighbor.tileSet.Tiles = neighbors;
            northNeighbor.backgrounds = sprites;
        }

        if (eastNeighbor != null)
        {
            List<Tile> neighbors = new List<Tile>();
            List<Sprite> sprites = new List<Sprite>(); 

            for (int i = 0; i < eastNeighbor.tileSet.Tiles.Count; i++)
            {
                if (eastNeighbor.tileSet.Tiles[i].WEST == tile.EAST)
                {
                    neighbors.Add(eastNeighbor.tileSet.Tiles[i]);
                    sprites.Add(eastNeighbor.backgrounds[i]);
                }
            }
            eastNeighbor.tileSet.Tiles = neighbors;
            eastNeighbor.backgrounds = sprites;
        }

        if (southNeighbor != null)
        {
            List<Tile> neighbors = new List<Tile>(16);
            List<Sprite> sprites = new List<Sprite>(16);   


            for (int i = 0; i < southNeighbor.tileSet.Tiles.Count; i++)
            {
                if (southNeighbor.tileSet.Tiles[i].NORTH == tile.SOUTH)
                {
                    neighbors.Add(southNeighbor.tileSet.Tiles[i]);
                    sprites.Add(southNeighbor.backgrounds[i]);
                }
            }
            southNeighbor.tileSet.Tiles = neighbors;
            southNeighbor.backgrounds = sprites;
        }

        if (westNeighbor != null)
        {
            List<Tile> neighbors = new List<Tile>();
            List<Sprite> sprites = new List<Sprite>(); 

            for (int i = 0; i < westNeighbor.tileSet.Tiles.Count; i++)
            {
                if (westNeighbor.tileSet.Tiles[i].EAST == tile.WEST)
                {
                    neighbors.Add(westNeighbor.tileSet.Tiles[i]);
                    sprites.Add(westNeighbor.backgrounds[i]);
                }
            }
            westNeighbor.tileSet.Tiles = neighbors;
            westNeighbor.backgrounds = sprites;
        }
    }
}