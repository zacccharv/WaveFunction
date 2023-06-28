using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaveFunctionManager))]
public class WaveFunctionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Reload Grid") && Application.isPlaying)
        {
            WaveFunctionManager manager = target as WaveFunctionManager;

            manager.Reroll();
        }
        if (GUILayout.Button("Spark") && Application.isPlaying)
        {
            WaveFunctionManager manager = target as WaveFunctionManager;

            manager.currentTiles[0].OnCollapsed();
        }
    }
}
