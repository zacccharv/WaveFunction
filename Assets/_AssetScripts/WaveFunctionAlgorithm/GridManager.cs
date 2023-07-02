using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int backtrackIndex = 0;
    public CommandManager CommandManager { get { return FindAnyObjectByType(typeof(CommandManager)) as CommandManager;  } }

    [field: SerializeField] public int DIM { get; set; }

    [HideInInspector] public int columnNumber = 10, rowNumber = 10;
    public float tileWidth = 1, tileHeight = 1;
    public List<CellBase> grid = new List<CellBase>();
    public List<CellBase> waveIndex = new List<CellBase>();
    public GameObject gridItem;
    public float offset = 5f;
    public int currentIndex = 0;

    void Awake()
    {
        columnNumber = DIM;
        rowNumber = DIM;
    }

    public void DrawGrid()
    {
        for (int i = 1; i < rowNumber + 1; i++)
        {
            for (int j = 1; j < columnNumber + 1; j++)
            {
                DrawSquare(i, j);
            }
        }
    }
    void DrawSquare(int rowNumber, int columnNumber)
    {
        float columnPos = columnNumber * tileWidth - offset;
        float rowPos = rowNumber * tileHeight - offset;

        GameObject obj = Instantiate(gridItem, new Vector2(columnPos, rowPos), Quaternion.identity, transform);

        obj.transform.localScale = new Vector3(tileWidth, tileHeight);

        obj.GetComponent<CellBase>().Position = new (columnNumber, rowNumber);
        obj.GetComponent<CellBase>().Index = columnNumber * rowNumber;

        if (currentIndex == 1)
        {
            obj.GetComponent<CellBase>().startTile = true;
        }
        grid.Add(obj.GetComponent<CellBase>());
    }
}
