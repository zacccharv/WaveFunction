using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EleminateCommand : TileCommand
{
    public override CellBase CellBase { get; set; }
    public override CommandManager CommandManager { get; set; }
    public override GridManager GridManager { get; set; }

    List<List<Tile>> _neighbors = new List<List<Tile>>();
    List<List<Sprite>> _sprites = new List<List<Sprite>>();
    
    public EleminateCommand(CellBase target, CommandManager commandManager, GridManager gridManager)
    {
        CellBase = target;

        if (target.NeighborCells[0] != null)
        {
            _neighbors.Add(target.NeighborCells[0].Cell.tileSet.Tiles);
        }
        if (target.NeighborCells[1] != null)
        {
            _neighbors.Add(target.NeighborCells[1].Cell.tileSet.Tiles);
        }
        if (target.NeighborCells[2] != null)
        {
            _neighbors.Add(target.NeighborCells[2].Cell.tileSet.Tiles);
        }
        if (target.NeighborCells[3] != null)
        {
            _neighbors.Add(target.NeighborCells[3].Cell.tileSet.Tiles);;
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
        if (CellBase.NeighborCells[0] != null)
        {
            CellBase.NeighborCells[0].Cell.tileSet.Tiles = _neighbors[0];            
        }
        if (CellBase.NeighborCells[1] != null)
        {
            CellBase.NeighborCells[1].Cell.tileSet.Tiles = _neighbors[1];      
        }
        if (CellBase.NeighborCells[2] != null && _neighbors.Count > 2)
        {      
            CellBase.NeighborCells[2].Cell.tileSet.Tiles = _neighbors[2];      
        }
        if (CellBase.NeighborCells[3] != null && _neighbors.Count > 3)
        {
            CellBase.NeighborCells[3].Cell.tileSet.Tiles = _neighbors[3];       
        }
    }
}
