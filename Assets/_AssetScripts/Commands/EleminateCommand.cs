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

        if (target.Cell.NeighborCells[0] != null)
        {
            _neighbors.Add(target.Cell.NeighborCells[0].tileSet.Tiles);
        }
        if (target.Cell.NeighborCells[1] != null)
        {
            _neighbors.Add(target.Cell.NeighborCells[1].tileSet.Tiles);
        }
        if (target.Cell.NeighborCells[2] != null)
        {
            _neighbors.Add(target.Cell.NeighborCells[2].tileSet.Tiles);
        }
        if (target.Cell.NeighborCells[3] != null)
        {
            _neighbors.Add(target.Cell.NeighborCells[3].tileSet.Tiles);;
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
        if (CellBase.Cell.NeighborCells[0] != null)
        {
            CellBase.Cell.NeighborCells[0].tileSet.Tiles = _neighbors[0];            
        }
        if (CellBase.Cell.NeighborCells[1] != null)
        {
            CellBase.Cell.NeighborCells[1].tileSet.Tiles = _neighbors[1];      
        }
        if (CellBase.Cell.NeighborCells[2] != null && _neighbors.Count > 2)
        {      
            CellBase.Cell.NeighborCells[2].tileSet.Tiles = _neighbors[2];      
        }
        if (CellBase.Cell.NeighborCells[3] != null && _neighbors.Count > 3)
        {
            CellBase.Cell.NeighborCells[3].tileSet.Tiles = _neighbors[3];       
        }
    }
}
