using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EleminateCommand : Command
{
    CurrentTile _target;
    CommandManager _commandManager;
    List<List<Tile>> _neighbors = new List<List<Tile>>();
    List<List<Sprite>> _sprites = new List<List<Sprite>>();
    public EleminateCommand(CurrentTile target, CommandManager commandManager)
    {
        _target = target;

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

        _commandManager = commandManager;
    }

    public override void Execute()
    {
        _target.RemoveNeighborsFromLists();
        _commandManager.PushCommand(this);
    }

    public override void Undo()
    {
        if (_target.northNeighbor != null)
        {
            _target.northNeighbor.tileSet.Tiles = _neighbors[0];            
            _target.northNeighbor.backgrounds = _sprites[0];
        }
        if (_target.eastNeighbor != null)
        {
            _target.eastNeighbor.tileSet.Tiles = _neighbors[1];            
            _target.eastNeighbor.backgrounds = _sprites[1];
        }
        if (_target.southNeighbor != null)
        {
            _target.southNeighbor.tileSet.Tiles = _neighbors[2];            
            _target.southNeighbor.backgrounds = _sprites[2];
        }
        if (_target.westNeighbor != null)
        {
            _target.westNeighbor.tileSet.Tiles = _neighbors[3];            
            _target.westNeighbor.backgrounds = _sprites[3];
        }
    }
}
