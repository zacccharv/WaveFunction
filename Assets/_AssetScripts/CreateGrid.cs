using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    public int columnNumber = 10, rowNumber = 10;
    public float width = 1, height = 1;
    [SerializeField] List<GameObject> grid = new List<GameObject>();
    public GameObject gridItem;
    private float offset = 5;

    // Start is called before the first frame update
    void Start()
    {
        offset = offset - (width/2);
        DrawGrid();
    }
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
        float columnPos = columnNumber * width - offset;
        float rowPos = rowNumber * height - offset;

        GameObject obj = Instantiate(gridItem, new Vector2(columnPos, rowPos), Quaternion.identity, transform);

        obj.transform.localScale = new Vector3(width, height);

        grid.Add(obj);
    }
}
