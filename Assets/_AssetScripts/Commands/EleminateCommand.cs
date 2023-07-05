using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EleminateCommand : TileCommand
{
    public override CellBase CellBase { get; set; }

    public override CommandManager CommandManager { get; set; }
    public override GridManager GridManager { get; set; }
    private List<Cell> localCells;

    List<List<Tile>> _neighbors = new List<List<Tile>>();
    
    public EleminateCommand(CellBase target, CommandManager commandManager, GridManager gridManager)
    {
        CellBase = target;

        localCells = new List<Cell>();

        CommandManager = commandManager;
    }

    public override void Execute()
    {
        foreach (var item in CellBase.NeighborCells)
        {
            if (item != null)
            {
                _neighbors.Add(new List<Tile>());
            }
            else
            {
                _neighbors.Add(null);
            }
        }

        if (_neighbors[0] != null)
        {
            _neighbors[0] = CellBase.NeighborCells[0].Cell.tileSet.Tiles;
        }
        if (_neighbors[1] != null)
        {
            _neighbors[1] = CellBase.NeighborCells[1].Cell.tileSet.Tiles;
        }
        if (_neighbors[2] != null)
        {
            _neighbors[2] = CellBase.NeighborCells[2].Cell.tileSet.Tiles;
        }
        if (_neighbors[3] != null)
        {
            _neighbors[3] = CellBase.NeighborCells[3].Cell.tileSet.Tiles;
        }

        RemoveNeighborsFromLists(_neighbors);

        CommandManager.PushTileCommand(this);
    }

    public override void Undo()
    {
        List<string> neighborCells = new List<string>() { "", "", "", ""};

        for (int i = 0; i < neighborCells.Count; i++)
        {
            if (_neighbors[i] != null)
            {
                neighborCells[i] = $"NeighborCells[{i}]";
            }
        }

        for (var i = 0; i < _neighbors.Count; i++)
        {
            if (neighborCells[i] != "")
            {
                CellBase.NeighborCells[i].Cell.tileSet.Tiles = _neighbors[i];
                CellBase.NeighborCells[i].CreateTileStrings(CellBase.NeighborCells[i].Cell.tileSet.Tiles); 
            }
        }
    }
    public void RemoveNeighborsFromLists(List<List<Tile>> NeighborCells)
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

            for (int i = 0; i < NeighborCells[0].Count; i++)
            {   
                if (CellBase.Cell.tile.NORTH == NeighborCells[0][i].SOUTH)
                {
                    neighbors.Add(NeighborCells[0][i]);
                }
            }
            
            CellBase.NeighborCells[0].Cell.tileSet.Tiles = neighbors;
            CellBase.NeighborCells[0].CreateTileStrings(neighbors);
        }

        if (neighborCells[1] != "")
        {
            List<Tile> neighbors = new List<Tile>();

            for (int i = 0; i < NeighborCells[1].Count; i++)
            {
                            
                if (CellBase.Cell.tile.EAST == NeighborCells[1][i].WEST )
                {
                    neighbors.Add(NeighborCells[1][i]);
                }
            }
            CellBase.NeighborCells[1].Cell.tileSet.Tiles = neighbors;
            CellBase.NeighborCells[1].CreateTileStrings(neighbors);
        }

        if (neighborCells[2] != "")
        {
            List<Tile> neighbors = new List<Tile>();

            for (int i = 0; i < NeighborCells[2].Count; i++)
            {             
                if (CellBase.Cell.tile.SOUTH == NeighborCells[2][i].NORTH)
                {
                    neighbors.Add(NeighborCells[2][i]);
            
                }
            }

            CellBase.NeighborCells[2].Cell.tileSet.Tiles = neighbors;
            CellBase.NeighborCells[2].CreateTileStrings(neighbors);
        }

        if (neighborCells[3] != "")
        {
            List<Tile> neighbors = new List<Tile>();

            for (int i = 0; i < NeighborCells[3].Count; i++)
            {
                if ( CellBase.Cell.tile.WEST == NeighborCells[3][i].EAST)
                {    
                    neighbors.Add(NeighborCells[3][i]);
                }
            }

            CellBase.NeighborCells[3].Cell.tileSet.Tiles = neighbors;
            CellBase.NeighborCells[3].CreateTileStrings(neighbors);
        }
    }
}
