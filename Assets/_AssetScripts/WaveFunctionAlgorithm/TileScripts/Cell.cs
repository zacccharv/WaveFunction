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
    public int currentTileIndex;
    public int whichNeighborAmI;
    public TileSet originalTileSet;

    public Cell(CellBase cellBase)
    {
        CellBase = cellBase;
        tileSet = new TileSet(WaveFunctionManager.backgrounds);
        originalTileSet = new TileSet(WaveFunctionManager.backgrounds);
    }

    // NEXT TILE TO COLLAPSE
    public Cell EntropyWave(List<CellBase> cells)
    {
        List<Cell> localCells = new List<Cell>();

        for (var i = 0; i < cells.Count ; i++)
        {
            if (cells[i] != null && CellBase.allowedNeighbors[i])
            {
                localCells.Add(cells[i].Cell);
            }
            else
            {
                localCells.Add(null);
            }
        }

        List<int> entropyAmounts = new List<int>();

        // generate list of neighbor tiles tile count
        void GetEntropy(Cell north = null, Cell east = null, Cell south = null, Cell west = null)
        {
            Cell[] tiles = { north, east, south, west };

            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] != null && !tiles[i].Collapsed && CellBase.allowedNeighbors[i] == true) { entropyAmounts.Add(tiles[i].tileSet.Tiles.Count); }
                if (tiles[i] == null || tiles[i].Collapsed) { CellBase.allowedNeighbors[i] = false; }
            }
        }
        GetEntropy(localCells[0], localCells[1], localCells[2], localCells[3]);

        int smallestEntropy = Mathf.Min(entropyAmounts.ToArray());

        // if all neighbors tiles depleted
        if (entropyAmounts.Count == 0 && CellBase.previousCellBase != null)
        {
            Debug.Log($"All neighbors depleted @ {Index}");
            return null;
        }

        // returns next tile
        Cell CollapsibleTile(Cell north = null, Cell east = null, Cell south = null, Cell west = null)
        {
            Cell[] tiles = { north, east, south, west };
            List<Cell> cells = new List<Cell>();

            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] != null && tiles[i].tileSet.Tiles.Count == smallestEntropy && !tiles[i].Collapsed)
                {
                    cells.Add(tiles[i]);
                }
            }

            if (cells.Count == 0)
            {
                return null;
            }

            int smallest = new System.Random().Next(0, cells.Count);

            return cells[smallest];
        }

        // if all neighbors collapsed
        if (CollapsibleTile(localCells[0], localCells[1], localCells[2], localCells[3]) == null)
        {
            Debug.Log($"All neighbors collapsed @ {Index}");
            return null;
        }

        Cell result = CollapsibleTile(localCells[0], localCells[1], localCells[2], localCells[3]);

        return result;
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
        Debug.Log($"rolled @ {Index}");
        tile = tileSet.Tiles[random];
        CellBase.spriteRenderer.sprite = tile.SPRITE;

        CellBase.sockets = new List<Socket>() { tile.NORTH, tile.EAST, tile.SOUTH, tile.WEST };
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
            List<Tile> originalNeighbors = new List<Tile>();

            for (int i = 0; i < NeighborCells[0].Cell.tileSet.Tiles.Count; i++)
            {   
                originalNeighbors.Add(NeighborCells[0].Cell.tileSet.Tiles[i]);

                if (tile.NORTH == NeighborCells[0].Cell.tileSet.Tiles[i].SOUTH)
                {
                    neighbors.Add(NeighborCells[0].Cell.tileSet.Tiles[i]);
                }
            }
            
            NeighborCells[0].Cell.tileSet.Tiles = neighbors;
            NeighborCells[0].CreateTileStrings(neighbors);
        }

        if (neighborCells[1] != "")
        {
            List<Tile> neighbors = new List<Tile>();
            List<Tile> originalNeighbors = new List<Tile>();

            for (int i = 0; i < NeighborCells[1].Cell.tileSet.Tiles.Count; i++)
            {
                originalNeighbors.Add(NeighborCells[1].Cell.tileSet.Tiles[i]);
                            
                if (NeighborCells[1].Cell.tileSet.Tiles[i].WEST == tile.EAST)
                {
                    NeighborCells[1].Cell.originalTileSet.Tiles = originalNeighbors; 
                    neighbors.Add(NeighborCells[1].Cell.tileSet.Tiles[i]);
                }
            }
            NeighborCells[1].Cell.tileSet.Tiles = neighbors;
            NeighborCells[1].CreateTileStrings(neighbors);
        }

        if (neighborCells[2] != "")
        {
            List<Tile> neighbors = new List<Tile>();
            List<Tile> originalNeighbors = new List<Tile>();

            for (int i = 0; i < NeighborCells[2].Cell.tileSet.Tiles.Count; i++)
            {
                originalNeighbors.Add(NeighborCells[2].Cell.tileSet.Tiles[i]);                
                if (NeighborCells[2].Cell.tileSet.Tiles[i].NORTH == tile.SOUTH)
                {
                    neighbors.Add(NeighborCells[2].Cell.tileSet.Tiles[i]);
            
                }
            }

            NeighborCells[2].Cell.tileSet.Tiles = neighbors;
            NeighborCells[2].CreateTileStrings(neighbors);
        }

        if (neighborCells[3] != "")
        {
            List<Tile> neighbors = new List<Tile>();
            List<Tile> originalNeighbors = new List<Tile>();
                

            for (int i = 0; i < NeighborCells[3].Cell.tileSet.Tiles.Count; i++)
            {
                NeighborCells[3].Cell.originalTileSet = NeighborCells[3].Cell.tileSet;
                if (NeighborCells[3].Cell.tileSet.Tiles[i].EAST == tile.WEST)
                {
                    originalNeighbors.Add(NeighborCells[3].Cell.tileSet.Tiles[i]);      
                    neighbors.Add(NeighborCells[3].Cell.tileSet.Tiles[i]);
                    NeighborCells[3].Cell.originalTileSet.Tiles = originalNeighbors; 
                }
            }

            NeighborCells[3].Cell.tileSet.Tiles = neighbors;
            NeighborCells[3].CreateTileStrings(neighbors);
        }
    }

    public override string ToString() => $"Cell => Index: {Index}, Position: {Position}";
}