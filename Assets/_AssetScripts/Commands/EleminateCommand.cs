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

        CellBase.Cell.RemoveNeighborsFromLists();

        CommandManager.PushTileCommand(this);
    }

    public override void Undo()
    {
        for (var i = 0; i < 4; i++)
        {
            if (_neighbors[i] != null)
            {
                // CellBase.Cell.tileSet.Tiles.RemoveAt(localCells[i].currentTileIndex);
                CellBase.NeighborCells[i].Cell.tileSet = new TileSet(CellBase.WaveFunctionManager.backgrounds);

                CellBase.NeighborCells[i].Cell.tileSet.Tiles = _neighbors[i];
                CellBase.NeighborCells[i].CreateTileStrings(CellBase.NeighborCells[i].Cell.tileSet.Tiles);

                Debug.Log(string.Join(", ", CellBase.NeighborCells[i].Cell.tileSet.Tiles));
            }       
        }
    }
}
