using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrackCommand : TileCommand
{
    public override Cell Cell { get; set; }
    public override CommandManager CommandManager { get; set; }
    public override GridManager GridManager { get; set; }

    public BackTrackCommand (CommandManager commandManager, GridManager gridManager)
    {
        CommandManager = commandManager;
        GridManager = gridManager;
    }

    public override void Execute()
    {
        GridManager.backtrackIndex += 1;
        CommandManager.UndoTileChunk(GridManager.backtrackIndex);

        GridManager.waveIndex[GridManager.waveIndex.Count - 2 - GridManager.backtrackIndex].Collapsed = false;

        GridManager.waveIndex[GridManager.waveIndex.Count - 2 - GridManager.backtrackIndex].OnCollapsed();

        GridManager.waveIndex.RemoveAt(GridManager.waveIndex.Count - GridManager.backtrackIndex);
    }

    public override void Undo()
    {
        throw new System.NotImplementedException();
    }
}
