using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : ICell
{
    public CellBase CellBase { get; set; }
    
    public GridManager GridManager => CellBase.GridManager;
    public CommandManager CommandManager => CellBase.CommandManager;
    public WaveFunctionManager WaveFunctionManager => CellBase.WaveFunctionManager;

    public Vector2 Position { get { return this.CellBase.Position; } set { this.CellBase.Position = value; } }
    public int Index { get { return this.CellBase.Index; } set { this.CellBase.Index = value; } }
    public bool Collapsed { get { return this.CellBase.Collapsed; } set { this.CellBase.Collapsed = value; } }
    public List<CellBase> NeighborCells {get { return this.CellBase.NeighborCells; } set { this.CellBase.NeighborCells = value; } } 
    
    public int cellFailIndex = 1;
    public bool failed;
    public Tile tile = new Tile();
    public TileSet tileSet;
    int NE = 1; int SE = 2; int SW = 3; int NW = 4;
    private int currentTileIndex;

    public Cell(CellBase cellBase)
    {
        CellBase = cellBase;
        tileSet = new TileSet(WaveFunctionManager.backgrounds);
    }

    public void EntropyWave(List<CellBase> cells)
    {
        List<Cell> localCells = new List<Cell>();

        foreach (var item in cells)
        {
            if (item != null)
            {
                localCells.Add(item.Cell);
            }
            else
            {
                localCells.Add(null);
            }
        }

        List<int> entropyAmounts = new List<int>();

        void GetEntropy(Cell north = null, Cell east = null, Cell south = null, Cell west = null)
        {
            Cell[] tileSet = { north, east, south, west };

            for (int i = 0; i < tileSet.Length; i++)
            {
                if (tileSet[i] != null && !tileSet[i].Collapsed) { entropyAmounts.Add(tileSet[i].tileSet.Tiles.Count); }
            }
        }
        GetEntropy(localCells[0], localCells[1], localCells[2], localCells[3]);

        if (entropyAmounts.Count == 0)
        {
            CommandManager.UndoLastTileCommand();
            CommandManager.UndoLastTileCommand();

            // collapse previous
            // CellBase.OnCollapsed();
            
            return;
        }

        int smallestEntropy = Mathf.Min(entropyAmounts.ToArray());

        Cell CollapsibleTile(Cell north = null, Cell east = null, Cell south = null, Cell west = null)
        {
            Cell[] tileSet = { north, east, south, west };
            List<Cell> cells = new List<Cell>();

            for (int i = 0; i < tileSet.Length; i++)
            {
                if (tileSet[i] != null && tileSet[i].tileSet.Tiles.Count == smallestEntropy && !tileSet[i].Collapsed)
                {
                    cells.Add(tileSet[i]);
                }
            }
            if (cells.Count == 0)
            {
                return null;
            }

            int smallest = new System.Random().Next(0, cells.Count);
            
            return cells[smallest];
        }

        if (CollapsibleTile(localCells[0], localCells[1], localCells[2], localCells[3]) == null)
        {
            return;
        }

        // Debug.Log(CollapsibleTile(localCells[0], localCells[1], localCells[2], localCells[3]).ToString());

        CollapsibleTile(localCells[0], localCells[1], localCells[2], localCells[3]).CellBase.previousCellBase = this.CellBase;
        CollapsibleTile(localCells[0], localCells[1], localCells[2], localCells[3]).CellBase.OnCollapsed();
    }
    public void SpiralWave()
        {
            WaveFunctionManager.waveDirection = tileDirection();

            if (tileDirection() == Direction.NORTH)
            {
                NeighborCells[0].OnCollapsed();
            }
            else if (tileDirection() == Direction.EAST)
            {
                NeighborCells[1].OnCollapsed();
            }
            else if (tileDirection() == Direction.SOUTH)
            {
                NeighborCells[2].OnCollapsed();
            } 
            else if(tileDirection() == Direction.WEST)
            {
                NeighborCells[3].OnCollapsed();
            }
        }
    public void WestToEastWave()
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

        return Direction.NONE;
    }
    List<bool> GetContacts()
    {
        bool northCollapse = NeighborCells[0] == null || NeighborCells[0].Collapsed;
        bool eastCollapse = NeighborCells[1] == null || NeighborCells[1].Collapsed;
        bool southCollapse = NeighborCells[2] == null || NeighborCells[2].Collapsed;
        bool westCollapse = NeighborCells[3] == null || NeighborCells[3].Collapsed;

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
    public void SetNeighbors()
    {
        List<CellBase> grid = GridManager.grid;
        
        bool NorthNeighborExists()
        {
            return Position.y < GridManager.DIM - 1;
        }
        bool EastNeighborExists()
        {
            return Position.x < GridManager.DIM - 1;
        }
        bool SouthNeighborExists()
        {
            return Position.y > 1;
        }        
        bool WestNeighborExists()
        {
            return Position.x > 1;
        }

        float indexOffset = 1;

        if (NorthNeighborExists())
        {
            NeighborCells[0] = grid[Mathf.RoundToInt((Position.x + 0 - indexOffset) + ((Position.y + 1 - indexOffset) * GridManager.DIM))];
        }
        else if (!NorthNeighborExists())
        {
            NeighborCells[0] = null;
        }

        if (EastNeighborExists())
        {
            NeighborCells[1] = grid[Mathf.RoundToInt((Position.x + 1 - indexOffset) + ((Position.y + 0 - indexOffset) * GridManager.DIM))];            
        }
        else if (!EastNeighborExists())
        {
            NeighborCells[1] = null;
        }

        if (SouthNeighborExists())
        {       
            NeighborCells[2] = grid[Mathf.RoundToInt((Position.x + 0 - indexOffset) + ((Position.y - 1 - indexOffset) * GridManager.DIM))];            
        }
        else if (!SouthNeighborExists())
        {
            NeighborCells[2] = null;
        }

        if (WestNeighborExists())
        {      
            NeighborCells[3] = grid[Mathf.RoundToInt((Position.x - 1 - indexOffset) + ((Position.y + 0 - indexOffset) * GridManager.DIM))];
        } 
        else if (!WestNeighborExists())
        {
            NeighborCells[3] = null;
        }  
    }  
    public void RollForTile(int random)
    {
        tile = tileSet.Tiles[random];
        CellBase.spriteRenderer.sprite = tile.SPRITE;

        CellBase.sockets = new List<Socket>() { tile.NORTH, tile.EAST, tile.SOUTH, tile.WEST };
        currentTileIndex = random;
        return;
    }
    public void RemoveNeighborsFromLists()
    {
        List<string> neighborCells = new List<string>() { "", "", "", ""};

        for (int i = 0; i < neighborCells.Count; i++)
        {
            if (NeighborCells[i] != null)
            {
                neighborCells[i] = $"NeighborCells[{i}]";
            }
        }

        if (neighborCells[0] != "") 
        {
            List<Tile> neighbors = new List<Tile>();

            for (int i = 0; i < NeighborCells[0].Cell.tileSet.Tiles.Count; i++)
            {   
                if (tile.NORTH == NeighborCells[0].Cell.tileSet.Tiles[i].SOUTH)
                {
                    neighbors.Add(NeighborCells[0].Cell.tileSet.Tiles[i]);
                }
            }
            NeighborCells[0].Cell.tileSet.Tiles = neighbors;
        }

        if (neighborCells[1] != "")
        {
            List<Tile> neighbors = new List<Tile>();

            for (int i = 0; i < NeighborCells[1].Cell.tileSet.Tiles.Count; i++)
            {
                if (NeighborCells[1].Cell.tileSet.Tiles[i].WEST == tile.EAST)
                {
                    neighbors.Add(NeighborCells[1].Cell.tileSet.Tiles[i]);
                }
            }
            NeighborCells[1].Cell.tileSet.Tiles = neighbors;
        }

        if (neighborCells[2] != "")
        {
            List<Tile> neighbors = new List<Tile>(16);


            for (int i = 0; i < NeighborCells[2].Cell.tileSet.Tiles.Count; i++)
            {
                if (NeighborCells[2].Cell.tileSet.Tiles[i].NORTH == tile.SOUTH)
                {
                    neighbors.Add(NeighborCells[2].Cell.tileSet.Tiles[i]);
                }
            }
            NeighborCells[2].Cell.tileSet.Tiles = neighbors;
        }

        if (neighborCells[3] != "")
        {
            List<Tile> neighbors = new List<Tile>();

            for (int i = 0; i < NeighborCells[3].Cell.tileSet.Tiles.Count; i++)
            {
                if (NeighborCells[3].Cell.tileSet.Tiles[i].EAST == tile.WEST)
                {
                    neighbors.Add(NeighborCells[3].Cell.tileSet.Tiles[i]);
                }
            }
            NeighborCells[3].Cell.tileSet.Tiles = neighbors;
        }

        tileSet.Tiles.RemoveAt(currentTileIndex);
    }
    // public void OnCollapsed()
    // {
    //     CellBase.OnCollapsed();
    // }

    public override string ToString() => $"Cell => Index: {Index}, Position: {Position}";
}