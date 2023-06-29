using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EleminateCommand : TileCommand
{
    public override CurrentTile CurrentTile { get; set; }
    public override CommandManager CommandManager { get; set; }
    public override GridManager GridManager { get; set; }

    List<List<Tile>> _neighbors = new List<List<Tile>>();
    List<List<Sprite>> _sprites = new List<List<Sprite>>();
    
    public EleminateCommand(CurrentTile target, CommandManager commandManager, GridManager gridManager)
    {
        CurrentTile = target;

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
        CurrentTile.RemoveNeighborsFromLists();
        CommandManager.PushTileCommand(this);
    }

    public override void Undo()
    {
        if (CurrentTile.northNeighbor != null)
        {
            CurrentTile.northNeighbor.tileSet.Tiles = _neighbors[0];            
            CurrentTile.northNeighbor.backgrounds = _sprites[0];
        }
        if (CurrentTile.eastNeighbor != null)
        {
            CurrentTile.eastNeighbor.tileSet.Tiles = _neighbors[1];            
            CurrentTile.eastNeighbor.backgrounds = _sprites[1];
        }
        if (CurrentTile.southNeighbor != null)
        {
            CurrentTile.southNeighbor.tileSet.Tiles = _neighbors[2];            
            CurrentTile.southNeighbor.backgrounds = _sprites[2];
        }
        if (CurrentTile.westNeighbor != null)
        {
            CurrentTile.westNeighbor.tileSet.Tiles = _neighbors[3];            
            CurrentTile.westNeighbor.backgrounds = _sprites[3];
        }
    }
}
