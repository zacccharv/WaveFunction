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

        if (target.neighborTiles[0] != null)
        {
            _neighbors.Add(target.neighborTiles[0].tileSet.Tiles);
            _sprites.Add(target.neighborTiles[0].backgrounds);
        }
        if (target.neighborTiles[1] != null)
        {
            _neighbors.Add(target.neighborTiles[1].tileSet.Tiles);
            _sprites.Add(target.neighborTiles[1].backgrounds);
        }
        if (target.neighborTiles[2] != null)
        {
            _neighbors.Add(target.neighborTiles[2].tileSet.Tiles);
            _sprites.Add(target.neighborTiles[2].backgrounds);
        }
        if (target.neighborTiles[3] != null)
        {
            _neighbors.Add(target.neighborTiles[3].tileSet.Tiles);
            _sprites.Add(target.neighborTiles[3].backgrounds);
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
        if (CurrentTile.neighborTiles[0] != null)
        {
            CurrentTile.neighborTiles[0].tileSet.Tiles = _neighbors[0];            
            CurrentTile.neighborTiles[0].backgrounds = _sprites[0];
        }
        if (CurrentTile.neighborTiles[1] != null)
        {
            CurrentTile.neighborTiles[1].tileSet.Tiles = _neighbors[1];            
            CurrentTile.neighborTiles[1].backgrounds = _sprites[1];
        }
        if (CurrentTile.neighborTiles[2] != null)
        {
            CurrentTile.neighborTiles[2].tileSet.Tiles = _neighbors[2];            
            CurrentTile.neighborTiles[2].backgrounds = _sprites[2];
        }
        if (CurrentTile.neighborTiles[3] != null)
        {
            CurrentTile.neighborTiles[3].tileSet.Tiles = _neighbors[3];            
            CurrentTile.neighborTiles[3].backgrounds = _sprites[3];
        }
    }
}
