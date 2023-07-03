using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int backtrackIndex = 0;
    public CommandManager CommandManager { get { return FindAnyObjectByType(typeof(CommandManager)) as CommandManager;  } }

    [field: SerializeField] public float DIM { get; set; }

    [HideInInspector] public float columnNumber, rowNumber;
    [HideInInspector] public float tileSize;
    public List<CellBase> grid = new List<CellBase>();
    public List<CellBase> waveIndex = new List<CellBase>();
    public GameObject gridItem;
    [HideInInspector] public float offset;
    int currentRow;
    int currentColumn;
    void Awake()
    {
        columnNumber = DIM;
        rowNumber = DIM;
        tileSize = 1 / (DIM/10);
    }
    public void DrawGrid()
    {
        for (int i = 1; i < rowNumber + 1; i++)
        {
            currentRow = i;
            for (int j = 1; j < columnNumber + 1; j++)
            {
                currentColumn = j;
                DrawSquare(i, j);
            }
        }
    }
    void DrawSquare(float currentRow, float currentColumn)
    {
        offset = Camera.main.orthographicSize;

        tileSize = 1 / (DIM/10);

        float columnPos = (currentColumn * tileSize - offset) - (tileSize/2);
        float rowPos = (currentRow * tileSize - offset) - (tileSize/2);

        GameObject obj = Instantiate(gridItem, new Vector2(columnPos, rowPos), Quaternion.identity, transform);
        obj.transform.localScale = new Vector3(tileSize, tileSize);

        CellBase cellBase = obj.GetComponent<CellBase>();
        cellBase.Position = new (currentColumn, currentRow);
        cellBase.Index = int.Parse($"{(currentColumn + ((currentRow - 1) * DIM))}");

        if (cellBase.Index == 1)
        {
            cellBase.startTile = true;
        }
        
        grid.Add(obj.GetComponent<CellBase>());
    }
}
