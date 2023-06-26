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

        if (GUILayout.Button("Reload Grid") && Application.isPlaying)
        {  
            CreateGrid createGrid = target as CreateGrid;

            createGrid.DrawGrid();
        }
    }
}
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

        if (GUILayout.Button("Reload Grid") && Application.isPlaying)
        {  
            CreateGrid createGrid = target as CreateGrid;

            createGrid.DrawGrid();
        }
    }
}
