using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseCommand : TileCommand
{
    public override CellBase CellBase { get; set; }
    public override CommandManager CommandManager { get; set; }
    public override GridManager GridManager { get; set; }

    public CollapseCommand(CellBase target, CommandManager commandManager, GridManager gridManager)
    {
        CellBase = target;
        CommandManager = commandManager;
        GridManager = gridManager;
    }

    public override void Execute()
    {   
        int randomNext = 0;
        System.Random random = new System.Random();

        if (CellBase.Cell.tileSet.Tiles.Count > 0)
        {
            randomNext = random.Next(0, CellBase.Cell.tileSet.Tiles.Count);

            CellBase.Cell.RollForTile(randomNext);

            GridManager.waveIndex.Add(CellBase);

            if (CellBase.sockets.Count == 0)
            {
                Debug.Log($"{CellBase.Index} Has Collapsed with no sockets");
            }

            if (CellBase.sockets.Count == 0)
            { 
                CellBase.Collapsed = true; 
            }

            CommandManager.PushTileCommand(this);
        }
        else
        {
            CellBase.failed = true;

            BackTrackCommand backTrackCommand = new BackTrackCommand(CellBase ,CommandManager, GridManager);

            backTrackCommand.Execute();

            if (GridManager.waveIndex.Count == GridManager.DIM * GridManager.DIM)
            {
                CellBase.StopAllCoroutines();
            }
        }
    }

    public override void Undo()
    {
        CellBase.Collapsed = false;
        CellBase.spriteRenderer.sprite = CellBase.initSprite;
        CellBase.Cell.tile = new Tile();
        CellBase.sockets = new List<Socket>();
        CellBase.Cell.tileSet = new TileSet(CellBase.WaveFunctionManager.backgrounds);
    }
}
