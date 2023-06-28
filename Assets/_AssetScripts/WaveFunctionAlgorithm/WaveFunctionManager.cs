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
        GetTiles();
    }
    public void GetTiles()
    {
        for (var i = 0; i < gridManager.transform.childCount; i++)
        {
            currentTiles.Add(gridManager.transform.GetChild(i).gameObject.GetComponent<CurrentTile>());
        }
    }
    public void Reroll()
    {
        foreach (var item in currentTiles)
        {
            item.collapsed = false;
            item.tile = new Tile();
            item.sockets = new List<Socket>();
            item.tileSet = new TileSet();
            item.backgrounds = this.backgrounds;

            item.spriteRenderer.sprite = backgrounds[0];
        }
    }
}