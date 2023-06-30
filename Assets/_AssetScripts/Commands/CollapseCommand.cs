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
            Cell.Collapsed = true;


            CommandManager.PushTileCommand(this);
        }
        else
        {
            // Cell.failed = true;

            // BackTrackCommand backTrackCommand = new BackTrackCommand(CommandManager, GridManager);

            // backTrackCommand.Execute();

            if (GridManager.waveIndex.Count == GridManager.columnNumber * GridManager.rowNumber)
            {
                Cell.StopAllCoroutines();
            }
        }
    }

    public override void Undo()
    {
        Cell.spriteRenderer.sprite = Cell.initSprite;

        Cell.sockets = new List<Socket>() { Socket.empty, Socket.empty, Socket.empty, Socket.empty };

        GridManager.waveIndex.Remove(Cell);
        Cell.Collapsed = false;
    }
}
