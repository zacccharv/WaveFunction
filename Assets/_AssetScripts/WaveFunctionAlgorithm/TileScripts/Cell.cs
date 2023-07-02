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
    public List<Cell> NeighborCells {get { return this.CellBase.NeighborCells; } set { this.CellBase.NeighborCells = value; } } 
    
    public int cellFailIndex = 1;
    public bool failed;
    public Tile tile = new Tile();
    public TileSet tileSet;
    int NE = 1; int SE = 2; int SW = 3; int NW = 4;

    public Cell(CellBase cellBase)
    {
        CellBase = cellBase;
        tileSet = new TileSet(WaveFunctionManager.backgrounds);

        tileSet.Sprites = WaveFunctionManager.backgrounds;

        CellBase.SetTileStrings(tileSet.Tiles);

        SetNeighbors();

        if (CellBase.startTile)
        {
            //CellBase.StartCoroutine(WaveFunction());
        }
    }
    public void RunCoroutine()
    {
        //CellBase.StartCoroutine(WaveFunction());
    }

    public IEnumerator WaveFunction()
    {
        
        /*if (GridManager.waveIndex.Count > (GridManager.DIM * GridManager.DIM) - 2)
        {
            Debug.Log("WaveFunction Cancelled");
            StopAllCoroutines();
        } */

        if (!Collapsed)
        {
            CollapseCommand collapse = new CollapseCommand(CellBase, CommandManager, GridManager);
            collapse.Execute();

            yield return null;

            EleminateCommand eleminate = new EleminateCommand(CellBase, CommandManager, GridManager);
            eleminate.Execute();

            yield return null;

            // WestToEastWave();

            // SpiralWave();

            EntropyWave();

            yield return null;
        }

        if (CellBase.sockets.Count == 0)
        {
            Debug.Log($"Uncollapsed at Index {Position} but shouldn't have been. After Entropy");
        }

        Collapsed = true;
        yield return null;

        CellBase.SetTileStrings(tileSet.Tiles);
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
        GetEntropy(NeighborCells[2], NeighborCells[1], NeighborCells[0], NeighborCells[3]);

        if (entropyAmounts.Count == 0)
        {
            BackTrackCommand backTrackCommand = new BackTrackCommand(CellBase, CommandManager, GridManager);
            backTrackCommand.Execute();
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

        if (CollapsibleTile(NeighborCells[0], NeighborCells[1], NeighborCells[2], NeighborCells[3]) == null)
        {
            Debug.Log(this.Position);
            return;
        }

        CollapsibleTile(NeighborCells[0], NeighborCells[1], NeighborCells[2], NeighborCells[3]).OnCollapsed();
    }
    void SpiralWave()
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
    void SetNeighbors()
    {
        List<CellBase> grid = GridManager.grid;
        
        bool NorthNeighborExists()
        {
            return Position.y < GridManager.rowNumber;
        }
        bool EastNeighborExists()
        {
            return Position.x < GridManager.columnNumber;
        }
        bool SouthNeighborExists()
        {
            return Position.y > 0;
        }        
        bool WestNeighborExists()
        {
            return Position.x > 0;
        }

        if (NorthNeighborExists())
        {
            NeighborCells[0].CellBase = grid.Find(f => f.Position == new Vector2(f.Position.x, f.Position.y+1));
        }
        if (EastNeighborExists())
        {
            NeighborCells[1].CellBase = grid.Find(f => f.Position == new Vector2(f.Position.x + 1, f.Position.y));            
        }
        if (SouthNeighborExists())
        {
            NeighborCells[2].CellBase = grid.Find(f => f.Position == new Vector2(f.Position.x, f.Position.y - 1));            
        }
        if (WestNeighborExists())
        {
            NeighborCells[3].CellBase = grid.Find(f => f.Position == new Vector2(f.Position.x - 1, f.Position.y));
        }   
    }  
    public void RollForTile(int random)
    {
        tile = tileSet.Tiles[random];

        CellBase.sockets = new List<Socket>() { tile.NORTH, tile.EAST, tile.SOUTH, tile.WEST };
        return;
    }
    public void RemoveNeighborsFromLists()
    {
        for (var i = 0; i < 4; i++)
        {
            NeighborSetComparer(i);
        }

        void NeighborSetComparer(int neighborSetIndex)
        {
            if (NeighborCells[neighborSetIndex] != null)
            {
                List<Tile> neighbors = new List<Tile>();

                TilesetComparer(neighborSetIndex, neighbors);

                NeighborCells[neighborSetIndex].tileSet.Tiles = neighbors;
                NeighborCells[neighborSetIndex].CellBase.SetTileStrings(neighbors);
            }
        }
        void TilesetComparer(int neighborSetIndex, List<Tile> neighbors)
        {
            for (int tileSetTileIndex = 0; tileSetTileIndex < NeighborCells[neighborSetIndex].tileSet.Tiles.Count; tileSetTileIndex++)
            {
                List<Socket> tileSetTileSockets = new List<Socket>() { NeighborCells[neighborSetIndex].tileSet.Tiles[tileSetTileIndex].NORTH, NeighborCells[neighborSetIndex].tileSet.Tiles[tileSetTileIndex].EAST, NeighborCells[neighborSetIndex].tileSet.Tiles[tileSetTileIndex].SOUTH, NeighborCells[neighborSetIndex].tileSet.Tiles[tileSetTileIndex].WEST };

                SocketComparer(neighborSetIndex, neighbors, tileSetTileIndex, tileSetTileSockets);
            }
        }
        void SocketComparer(int neighborSetIndex, List<Tile> neighbors, int tileSetTileIndex, List<Socket> tileSetTileSockets)
        {
            for (var socketIndex = 0; socketIndex < 4; socketIndex++)
            {
                if (tileSetTileSockets[socketIndex] == CellBase.sockets[socketIndex])
                {
                    neighbors.Add(NeighborCells[neighborSetIndex].tileSet.Tiles[socketIndex]);
                }
            }
        }

    }
    public void OnCollapsed()
    {
        CellBase.OnCollapsed();
    }
}