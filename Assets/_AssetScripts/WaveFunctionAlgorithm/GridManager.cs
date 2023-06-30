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
    public List<Cell> grid = new List<Cell>();
    public List<Cell> waveIndex = new List<Cell>();
    public GameObject gridItem;
    public float offset = 4.5f;
    public int currentIndex = 0;

    void Awake()
    {
        columnNumber = DIM;
        rowNumber = DIM;
    }

    public void DrawGrid()
    {
        for (int i = 0; i < rowNumber; i++)
        {
            for (int j = 0; j < columnNumber; j++)
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
        currentIndex++;

        obj.GetComponent<CellBase>().Index = currentIndex;

        if (currentIndex == 1)
        {
            obj.GetComponent<Cell>().startTile = true;
        }

        grid.Add(obj.GetComponent<Cell>());
    }
}
