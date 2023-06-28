using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionManager : MonoBehaviour
{
    public GridManager gridManager;
    public List<CurrentTile> currentTiles = new List<CurrentTile>();
    public TileSet tileSet = new TileSet();
    public List<Sprite> backgrounds = new List<Sprite>();

    void Awake()
    {
        ResetTiles();
    }
    void ResetTiles()
    {
        for (var i = 0; i < gridManager.transform.childCount; i++)
        {
            currentTiles.Add(gridManager.transform.GetChild(i).gameObject.GetComponent<CurrentTile>());
        }
    }
}