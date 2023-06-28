using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int columnNumber = 10, rowNumber = 10;
    
    public float tileWidth = 1, tileHeight = 1;
    public List<CurrentTile> grid = new List<CurrentTile>();
    public GameObject gridItem;
    public float offset = 4.5f;
    public int currentIndex = 0;
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

        obj.GetComponent<Cell>().index = currentIndex;

        grid.Add(obj.GetComponent<CurrentTile>());
    }
}
