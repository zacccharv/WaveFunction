using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollCommand : TileCommand
{
    public override CurrentTile CurrentTile { get; set; }
    public override CommandManager CommandManager { get; set; }
    public override GridManager GridManager { get; set; }

    public RollCommand(CurrentTile target, CommandManager commandManager, GridManager gridManager)
    {
        CurrentTile = target;
        CommandManager = commandManager;
        GridManager = gridManager;
    }

    public override void Execute()
    {   
        int randomNext = 0;
        System.Random random = new System.Random();

        if (CurrentTile.backgrounds.Count > 0)
        {
            randomNext = random.Next(0, CurrentTile.backgrounds.Count);

            
            if (CurrentTile.index == 1)
            {
               CurrentTile.RollForTile(15);
            }
            else
            {
                CurrentTile.RollForTile(randomNext);
            }

            GridManager.waveIndex.Add(CurrentTile);
            CurrentTile.collapsed = true;


            CommandManager.PushTileCommand(this);
        }
        else
        {
            // CurrentTile.failed = true;

            // BackTrackCommand backTrackCommand = new BackTrackCommand(CommandManager, GridManager);

            // backTrackCommand.Execute();

            if (GridManager.waveIndex.Count == GridManager.columnNumber * GridManager.rowNumber)
            {
                CurrentTile.StopAllCoroutines();
            }
        }
    }

    public override void Undo()
    {
        CurrentTile.spriteRenderer.sprite = CurrentTile.initSprite;

        CurrentTile.sockets = new List<Socket>() { Socket.empty, Socket.empty, Socket.empty, Socket.empty };

        GridManager.waveIndex.Remove(CurrentTile);
        CurrentTile.collapsed = false;
    }
}
