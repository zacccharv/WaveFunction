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

            createGrid.offset = createGrid.tileWidth * (createGrid.columnNumber/2) - (createGrid.tileWidth/2);

            if (createGrid.grid.Count > 0)
            {
                return;
            }

            createGrid.DrawGrid();
        }
        if (GUILayout.Button("Reset") && !Application.isPlaying)
        {
            GridManager createGrid = target as GridManager;

            foreach (var item in createGrid.grid)
            {
                DestroyImmediate(item.gameObject);
            }

            createGrid.currentIndex = 0;
            
            createGrid.grid.Clear();
        }
    }
}
