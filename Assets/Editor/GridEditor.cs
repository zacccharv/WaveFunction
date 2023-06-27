using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CreateGrid))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Reload Grid") && !Application.isPlaying)
        {  
            CreateGrid createGrid = target as CreateGrid;

            if (createGrid.grid.Count > 0)
            {
                return;
            }

            createGrid.offset = createGrid.offset - (createGrid.tileWidth/2);

            createGrid.DrawGrid();
        }
    }
}
