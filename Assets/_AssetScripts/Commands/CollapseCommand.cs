using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseCommand : TileCommand
{
    public override Cell Cell { get; set; }
    public override CommandManager CommandManager { get; set; }
    public override GridManager GridManager { get; set; }

    public CollapseCommand(Cell target, CommandManager commandManager, GridManager gridManager)
    {
        Cell = target;
        CommandManager = commandManager;
        GridManager = gridManager;
    }

    public override void Execute()
    {   
        int randomNext = 0;
        System.Random random = new System.Random();

        if (Cell.backgrounds.Count > 0)
        {
            randomNext = random.Next(0, Cell.backgrounds.Count);

            Cell.RollForTile(randomNext);

            GridManager.waveIndex.Add(Cell);

            if (Cell.sockets.Count == 0)
            {
                Debug.Log($"{Cell.Index} Has Collapsed with no sockets");
            }

            if (Cell.sockets.Count == 0)
            { 
                Cell.Collapsed = true; 
            }

            CommandManager.PushTileCommand(this);
        }
        else
        {
            Cell.failed = true;

            BackTrackCommand backTrackCommand = new BackTrackCommand(Cell ,CommandManager, GridManager);

            backTrackCommand.Execute();

            if (GridManager.waveIndex.Count == GridManager.DIM * GridManager.DIM)
            {
                Cell.StopAllCoroutines();
            }
        }
    }

    public override void Undo()
    {
        Cell.Collapsed = false;
        Cell.spriteRenderer.sprite = Cell.initSprite;
        Cell.tile = new Tile();
        Cell.sockets = new List<Socket>();
        Cell.tileSet = new TileSet();
        Cell.backgrounds = Cell.WaveFunctionManager.backgrounds;
    }
}
