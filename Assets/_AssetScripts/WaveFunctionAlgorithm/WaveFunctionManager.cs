using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionManager : MonoBehaviour
{
    public CommandManager CommandManager { get { return FindAnyObjectByType<CommandManager>(); } }
    public static Direction waveDirection = Direction.NONE;

    public GridManager gridManager;
    public List<Cell> Cells = new List<Cell>();
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
            Cells.Add(gridManager.transform.GetChild(i).gameObject.GetComponent<Cell>());
        }
    }
    public void Reroll()
    {
        foreach (var item in Cells)
        {
            item.Collapsed = false;
            item.tile = new Tile();
            item.sockets = new List<Socket>();
            item.tileSet = new TileSet();
            item.backgrounds = this.backgrounds;

            item.spriteRenderer.sprite = backgrounds[0];
        }
    }
}