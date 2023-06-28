using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Jobs;
using UnityEngine;

public class CurrentTile : Cell
{
    public bool startTile;
    Tile tile = new Tile();
    public TileSet tileSet = new TileSet();
    public List<Socket> sockets = new List<Socket>();

    public CurrentTile northNeighbor;

    public CurrentTile eastNeighbor;

    public CurrentTile southNeighbor;

    public CurrentTile westNeighbor;
    
    [SerializeField] List<Sprite> backgrounds;
    public float size;
    private SpriteRenderer spriteRenderer;


    private void OnEnable() 
    {
        tileCollapseEvent += RunCoroutine;
    }
    private void OnDisable() 
    {
        tileCollapseEvent -= RunCoroutine;
    }

    private void Awake() 
    {
        NeighborChecks();
        if (startTile)
        {
            for (var i = 0; i < tileSet.Tiles.Count - 1; i++)
            {
                Debug.Log($"{tileSet.Tiles[i].ToString()}, index = {i}");
            }

            RunCoroutine();
        }
    }

    private void RunCoroutine()
    {
        StartCoroutine(RunMethods());
    }

    public IEnumerator RunMethods()
    {
        if (!collapsed)
        {        
            RemoveNeighborsFromLists();
            yield return new WaitForEndOfFrame();

            RollForTile();
            yield return new WaitForEndOfFrame();

            if (northNeighbor != null && !northNeighbor.collapsed)
            {
                northNeighbor.OnCollapsed();
            }
            else if (eastNeighbor != null && !eastNeighbor.collapsed)
            {
                eastNeighbor.OnCollapsed();
            }
            else if (southNeighbor != null && !southNeighbor.collapsed)
            {
                southNeighbor.OnCollapsed();
            }     
            else if (westNeighbor != null && !westNeighbor.collapsed)
            {
                westNeighbor.OnCollapsed();
            }                    
            yield return new WaitForEndOfFrame();
        }

        collapsed = true;
        yield return null;
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

            return;
        }
        
        Debug.Log("failed");
        RollForTile();
    }
    private void SetCurrentTile(int random)
    {
        tile = tileSet.Tiles[random];

        sockets = new List<Socket>() { tile.NORTH, tile.EAST, tile.SOUTH, tile.WEST };

        GetComponent<SpriteRenderer>().sprite = backgrounds[random];
    }
    private void RemoveNeighborsFromLists()
    {
        if (northNeighbor != null) 
        {
            for (int i = 0; i < northNeighbor.tileSet.Tiles.Count - 1; i++)
            {
                if (northNeighbor.tileSet.Tiles[i].SOUTH != tile.NORTH)
                {
                    northNeighbor.tileSet.Tiles.Remove(northNeighbor.tileSet.Tiles[i]);
                    northNeighbor.backgrounds.RemoveAt(i);
                }
            }
        }

        if (eastNeighbor != null)
        {
            for (int i = 0; i < eastNeighbor.tileSet.Tiles.Count - 1; i++)
            {
                if (eastNeighbor.tileSet.Tiles[i].WEST != tile.EAST)
                {
                    eastNeighbor.tileSet.Tiles.Remove(eastNeighbor.tileSet.Tiles[i]);
                    eastNeighbor.backgrounds.RemoveAt(i);
                }
            }
        }

        if (southNeighbor != null)
        {
            for (int i = 0; i < southNeighbor.tileSet.Tiles.Count - 1; i++)
            {
                if (southNeighbor.tileSet.Tiles[i].NORTH != tile.SOUTH)
                {
                    southNeighbor.tileSet.Tiles.Remove(southNeighbor.tileSet.Tiles[i]);
                    southNeighbor.backgrounds.RemoveAt(i);
                }
            }
        }

        if (westNeighbor != null)
        {
            for (int i = 0; i < westNeighbor.tileSet.Tiles.Count - 1; i++)
            {
                if (westNeighbor.tileSet.Tiles[i].EAST != tile.WEST)
                {
                    westNeighbor.tileSet.Tiles.Remove(westNeighbor.tileSet.Tiles[i]);
                    westNeighbor.backgrounds.RemoveAt(i);
                }
            }
        }
    }
}