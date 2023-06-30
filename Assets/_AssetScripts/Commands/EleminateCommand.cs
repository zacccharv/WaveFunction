using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EleminateCommand : TileCommand
{
    public override Cell Cell { get; set; }
    public override CommandManager CommandManager { get; set; }
    public override GridManager GridManager { get; set; }

    List<List<Tile>> _neighbors = new List<List<Tile>>();
    List<List<Sprite>> _sprites = new List<List<Sprite>>();
    
    public EleminateCommand(Cell target, CommandManager commandManager, GridManager gridManager)
    {
        Cell = target;

        if (target.northNeighbor != null)
        {
            _neighbors.Add(target.northNeighbor.tileSet.Tiles);
            _sprites.Add(target.northNeighbor.backgrounds);
        }
        if (target.eastNeighbor != null)
        {
            _neighbors.Add(target.eastNeighbor.tileSet.Tiles);
            _sprites.Add(target.eastNeighbor.backgrounds);
        }
        if (target.southNeighbor != null)
        {
            _neighbors.Add(target.southNeighbor.tileSet.Tiles);
            _sprites.Add(target.southNeighbor.backgrounds);
        }
        if (target.westNeighbor != null)
        {
            _neighbors.Add(target.westNeighbor.tileSet.Tiles);
            _sprites.Add(target.westNeighbor.backgrounds);
        }

        CommandManager = commandManager;
    }


    public override void Execute()
    {
        Cell.RemoveNeighborsFromLists();
        CommandManager.PushTileCommand(this);
    }

    public override void Undo()
    {
        if (Cell.northNeighbor != null)
        {
            Cell.northNeighbor.tileSet.Tiles = _neighbors[0];            
            Cell.northNeighbor.backgrounds = _sprites[0];
        }
        if (Cell.eastNeighbor != null)
        {
            Cell.eastNeighbor.tileSet.Tiles = _neighbors[1];            
            Cell.eastNeighbor.backgrounds = _sprites[1];
        }
        if (Cell.southNeighbor != null && _neighbors.Count > 2)
        {      
            Cell.southNeighbor.tileSet.Tiles = _neighbors[2];            
            Cell.southNeighbor.backgrounds = _sprites[2];
        }
        if (Cell.westNeighbor != null && _neighbors.Count > 3)
        {
            Cell.westNeighbor.tileSet.Tiles = _neighbors[3];            
            Cell.westNeighbor.backgrounds = _sprites[3];
        }
    }
}
