using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollCommand : Command
{
    CurrentTile _target;
    CommandManager _commandManager;
    public RollCommand(CurrentTile target, CommandManager commandManager)
    {
        _target = target;
        _commandManager = commandManager;
    }
    public override void Execute()
    {
        _target.RollForTile();
        _commandManager.PushCommand(this);
    }

    public override void Undo()
    {
        _target.spriteRenderer.sprite = _target.initSprite;

        _target.sockets = new List<Socket>() { Socket.empty, Socket.empty, Socket.empty, Socket.empty };
    }
}
