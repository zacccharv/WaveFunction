using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public Stack<TileCommand> TileCommandStack = new Stack<TileCommand>();
    public void PushTileCommand(TileCommand tileCommand)
    {
        TileCommandStack.Push(tileCommand);
    }    
    public void UndoLastTileCommand()
    {
        if (TileCommandStack.Count == 0) return;

        TileCommand lastCommand = TileCommandStack.Pop();
        lastCommand.Undo();        
    }
    
    public void UndoTileChunk(int chunkCount)
    {
        if (TileCommandStack.Count < chunkCount) return;

        for (var i = 0; i < chunkCount; i++)
        {
            TileCommand lastCommand = TileCommandStack.Pop();
            lastCommand.Undo();      
            lastCommand = TileCommandStack.Pop();
            lastCommand.Undo();      
        }
    }
}
