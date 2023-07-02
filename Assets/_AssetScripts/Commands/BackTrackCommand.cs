using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrackCommand : TileCommand
{
    public override CellBase CellBase { get; set; }
    public override CommandManager CommandManager { get; set; }
    public override GridManager GridManager { get; set; }

    public BackTrackCommand(CellBase cell, CommandManager commandManager, GridManager gridManager)
    {
        CellBase = cell;
        CommandManager = commandManager;
        GridManager = gridManager;
    }

    public override void Execute()
    {
        CellBase.Cell.cellFailIndex += 1;

        CommandManager.UndoTileChunk(CellBase.Cell.cellFailIndex);
        GridManager.waveIndex.Remove(GridManager.waveIndex[GridManager.waveIndex.Count - 1]);

        GridManager.waveIndex[GridManager.waveIndex.Count - 1].Collapsed = false;

        GridManager.waveIndex[GridManager.waveIndex.Count - 1].OnCollapsed();
    }

    public override void Undo()
    {
        throw new System.NotImplementedException();
    }
}
