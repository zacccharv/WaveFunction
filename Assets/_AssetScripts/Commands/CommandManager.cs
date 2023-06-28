using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public Stack<Command> CommandStack = new Stack<Command>();
    public void PushCommand(Command command)
    {
        CommandStack.Push(command);
    }

    public void UndoLastCommand()
    {
        if (CommandStack.Count == 0) return;

        Command lastCommand = CommandStack.Pop();
        lastCommand.Undo();
    }
}
