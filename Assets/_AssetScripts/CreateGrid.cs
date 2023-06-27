using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    public static event System.Action gridLoaded;
    public int columnNumber = 10, rowNumber = 10;
    public float tileWidth = 1, tileHeight = 1;
    public List<GameObject> grid = new List<GameObject>();
    public GameObject gridItem;
    public float offset = 5;
    public void DrawGrid()
    {
        foreach (var item in grid)
        {
            Destroy(item);
        }

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

        grid.Add(obj);
    }
}
