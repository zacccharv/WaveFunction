using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridManager))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Reload Grid") && !Application.isPlaying)
        {  
            GridManager createGrid = target as GridManager;

            if (createGrid.grid.Count < 1)
            {
                createGrid.DrawGrid();
            }
            
        }
        if (GUILayout.Button("Reset") && !Application.isPlaying)
        {
            GridManager createGrid = target as GridManager;

            foreach (var item in createGrid.grid)
            {
                if (item != null)
                {
                    DestroyImmediate(item.gameObject);
                }
            }
            
            createGrid.grid.Clear();
        }
    }
}
