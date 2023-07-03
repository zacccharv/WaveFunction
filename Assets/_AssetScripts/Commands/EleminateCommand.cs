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

        foreach (var item in target.NeighborCells)
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

        if (localCells[0] != null)
        {
            localCells[0].tileSet.Tiles = target.NeighborCells[0].Cell.tileSet.Tiles;
        }
        if (localCells[1] != null)
        {
            localCells[1].tileSet.Tiles = target.NeighborCells[1].Cell.tileSet.Tiles;
        }
        if (localCells[2] != null)
        {
            localCells[2].tileSet.Tiles = target.NeighborCells[2].Cell.tileSet.Tiles;
        }
        if (localCells[3] != null)
        {
            localCells[3].tileSet.Tiles = target.NeighborCells[3].Cell.tileSet.Tiles;
        }

        CommandManager = commandManager;
    }

    public override void Execute()
    {
        CellBase.Cell.RemoveNeighborsFromLists();

        CommandManager.PushTileCommand(this);
    }

    public override void Undo()
    {
        for (var i = 0; i < 4; i++)
        {
            if (localCells[i] != null)
            {
                CellBase.NeighborCells[i].Cell.tileSet.Tiles = localCells[i].tileSet.Tiles;
                CellBase.NeighborCells[i].CreateTileStrings(CellBase.NeighborCells[i].Cell.tileSet.Tiles);
            }       
        }
        
    }
}
