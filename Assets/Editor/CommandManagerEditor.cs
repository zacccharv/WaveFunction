using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CommandManager))]
public class CommandManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Undo"))
        {  
            CommandManager manager = target as CommandManager;

            manager.UndoLastTileCommand();
            manager.UndoLastTileCommand();
        }
    }
}
