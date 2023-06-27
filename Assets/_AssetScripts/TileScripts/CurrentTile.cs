using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CurrentTile : Cell
{
    Tile tile = new Tile();
    public NeighborTiles tileSet = new NeighborTiles();
    public List<Socket> sockets = new List<Socket>();

    public CurrentTile northNeighbor;

    public CurrentTile eastNeighbor;

    public CurrentTile southNeighbor;

    public CurrentTile westNeighbor;
    
    [SerializeField] List<Sprite> backgrounds;
    public float size;

    private void OnEnable() 
    {
        tileCollapseEvent += RunMethods;
    }
    private void OnDisable() 
    {
        tileCollapseEvent -= RunMethods;
    }
    void Start()
    {
        NeighborChecks();

        if (index == 1)
        {
            RunMethods();
        }
    }

    private void RunMethods()
    {
        RemoveNeighborsFromLists();
        RollForTile();
    }

    void NeighborChecks()
    {
        List<CurrentTile> grid = GridManager.grid;

        bool GetNorthEdgeCheck()
        {
            return Mathf.Ceil(index / GridManager.columnNumber) == GridManager.columnNumber - 1 || index == GridManager.columnNumber * GridManager.rowNumber;
        }
        bool GetEastColumnCheck()
        {
            return index % GridManager.columnNumber == 0;
        }
        bool GetSouthEdgeCheck()
        {
            return Mathf.Ceil(index / GridManager.columnNumber) == 0 || index == GridManager.columnNumber;
        }        
        bool GetWestColumnEdgeCheck()
        {
            return index % GridManager.columnNumber == 1;
        }


        if (!GetNorthEdgeCheck())
        {
            northNeighbor = grid[(index - 1) + GridManager.columnNumber];
        }
        if (!GetEastColumnCheck())
        {
            eastNeighbor = grid[(index - 1) + 1];
        }
        if (!GetSouthEdgeCheck())
        {
            southNeighbor = grid[(index - 1) - GridManager.columnNumber];
        }
        if (!GetWestColumnEdgeCheck())
        {
            westNeighbor = grid[(index - 1)-1];
        }        
    }
    void RollForTile()
    {
        int random = UnityEngine.Random.Range(0, tileSet.Tiles.Count);

        if (random < tileSet.Tiles.Count)
        {
            SetCurrentTile(random);

            if (northNeighbor != null) northNeighbor.OnCollapsed();
            if (eastNeighbor != null) eastNeighbor.OnCollapsed();
            if (southNeighbor != null) southNeighbor.OnCollapsed();
            if (westNeighbor != null) westNeighbor.OnCollapsed();

            return;
        }
        
        Debug.Log("failed");
        RollForTile();
        
    }
    private void SetCurrentTile(int random)
    {
        tile = tileSet.Tiles[random];

        sockets.Capacity = 4;
        sockets.AddRange(new List<Socket>() { tile.NORTH, tile.EAST, tile.SOUTH, tile.WEST });

        GetComponent<SpriteRenderer>().sprite = backgrounds[random];
        
        collapsed = true;
    }
    private void RemoveNeighborsFromLists()
    {
        for (int i = 0; i < northNeighbor.tileSet.Tiles.Count - 1; i++)
        {
            if (northNeighbor.tileSet.Tiles[i].SOUTH != tile.NORTH)
            {
                northNeighbor.tileSet.Tiles.Remove(northNeighbor.tileSet.Tiles[i]);
                northNeighbor.backgrounds.RemoveAt(i);
            }
        }
        for (int i = 0; i < eastNeighbor.tileSet.Tiles.Count - 1; i++)
        {
            if (eastNeighbor.tileSet.Tiles[i].WEST != tile.EAST)
            {
                eastNeighbor.tileSet.Tiles.Remove(eastNeighbor.tileSet.Tiles[i]);
                eastNeighbor.backgrounds.RemoveAt(i);
            }
        }
        for (int i = 0; i < southNeighbor.tileSet.Tiles.Count - 1; i++)
        {
            if (southNeighbor.tileSet.Tiles[i].WEST != tile.EAST)
            {
                southNeighbor.tileSet.Tiles.Remove(southNeighbor.tileSet.Tiles[i]);
                southNeighbor.backgrounds.RemoveAt(i);
            }
        }
        for (int i = 0; i < westNeighbor.tileSet.Tiles.Count - 1; i++)
        {
            if (westNeighbor.tileSet.Tiles[i].WEST != tile.EAST)
            {
                westNeighbor.tileSet.Tiles.Remove(westNeighbor.tileSet.Tiles[i]);
                westNeighbor.backgrounds.RemoveAt(i);
            }
        }
    }

}
