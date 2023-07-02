using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionManager : MonoBehaviour
{
    public CommandManager CommandManager { get { return FindAnyObjectByType<CommandManager>(); } }
    public static Direction waveDirection = Direction.NONE;

    public GridManager gridManager;
    public List<CellBase> Cells = new List<CellBase>();
    public TileSet tileSet;
    public List<Sprite> backgrounds = new List<Sprite>();

    void Awake()
    {
        tileSet = new TileSet(backgrounds);
        GetTiles();
    }
    public void GetTiles()
    {
        for (var i = 0; i < gridManager.transform.childCount; i++)
        {
            Cells.Add(gridManager.transform.GetChild(i).gameObject.GetComponent<CellBase>());
        }
    }
    public void Reroll()
    {
        foreach (var item in Cells)
        {
            item.Collapsed = false;
            item.Cell.tile = new Tile();
            item.sockets = new List<Socket>();
            item.Cell.tileSet = new TileSet(backgrounds);

            item.spriteRenderer.sprite = item.initSprite;
        }
    }
}