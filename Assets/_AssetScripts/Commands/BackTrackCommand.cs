using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrackCommand : TileCommand
{
    public override Cell Cell { get; set; }
    public override CommandManager CommandManager { get; set; }
    public override GridManager GridManager { get; set; }

    public BackTrackCommand (Cell cell, CommandManager commandManager, GridManager gridManager)
    {
        Cell = cell;
        CommandManager = commandManager;
        GridManager = gridManager;
    }

    public override void Execute()
    {
        Cell.cellFailIndex += 1;

        CommandManager.UndoTileChunk(Cell.cellFailIndex);
        GridManager.waveIndex.Remove(GridManager.waveIndex[GridManager.waveIndex.Count - 1]);

        GridManager.waveIndex[GridManager.waveIndex.Count - 1].Collapsed = false;

        GridManager.waveIndex[GridManager.waveIndex.Count - 1].OnCollapsed();
    }

    public override void Undo()
    {
        throw new System.NotImplementedException();
    }
}
