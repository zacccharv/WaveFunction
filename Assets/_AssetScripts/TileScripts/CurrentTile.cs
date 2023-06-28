using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    
    public List<Sprite> backgrounds = new List<Sprite>();
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        NeighborChecks();

        Debug.Log(backgrounds.Count);
        Debug.Log(tileSet.Tiles.Count);

        if (startTile)
        {
            StartCoroutine(RunMethods());
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
            RollForTile();
            yield return null;     

            RemoveNeighborsFromLists();
            yield return null;

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
            yield return null;
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

        SetCurrentTile(random);
        return;
    }
    private void SetCurrentTile(int random)
    {
        if (backgrounds.Count-1 < random)
        {
            StopAllCoroutines();
            return;
        }

        spriteRenderer.sprite = backgrounds[random];
        tile = tileSet.Tiles[random];

        sockets = new List<Socket>() { tile.NORTH, tile.EAST, tile.SOUTH, tile.WEST };
    }
    private void RemoveNeighborsFromLists()
    {
        if (northNeighbor != null) 
        {
            List<Tile> neighbors = new List<Tile>();
            List<Sprite> sprites = new List<Sprite>();

            for (int i = 0; i < northNeighbor.tileSet.Tiles.Count - 1; i++)
            {   

                if (tile.NORTH == northNeighbor.tileSet.Tiles[i].SOUTH)
                {
                    neighbors.Add(northNeighbor.tileSet.Tiles[i]);
                    sprites.Add(northNeighbor.backgrounds[i]);
                }
            }
            northNeighbor.tileSet.Tiles = neighbors;
            northNeighbor.backgrounds = sprites;
        }

        if (eastNeighbor != null)
        {
            List<Tile> neighbors = new List<Tile>();
            List<Sprite> sprites = new List<Sprite>(); 

            for (int i = 0; i < eastNeighbor.tileSet.Tiles.Count - 1; i++)
            {
                if (eastNeighbor.tileSet.Tiles[i].WEST == tile.EAST)
                {
                    neighbors.Add(eastNeighbor.tileSet.Tiles[i]);
                    sprites.Add(eastNeighbor.backgrounds[i]);
                }
            }
            eastNeighbor.tileSet.Tiles = neighbors;
            eastNeighbor.backgrounds = sprites;
        }

        if (southNeighbor != null)
        {
            List<Tile> neighbors = new List<Tile>(16);
            List<Sprite> sprites = new List<Sprite>(16);   


            for (int i = 0; i < southNeighbor.tileSet.Tiles.Count - 1; i++)
            {
                if (southNeighbor.tileSet.Tiles[i].NORTH == tile.SOUTH)
                {
                    neighbors.Add(southNeighbor.tileSet.Tiles[i]);
                    sprites.Add(southNeighbor.backgrounds[i]);
                }
            }
            southNeighbor.tileSet.Tiles = neighbors;
            southNeighbor.backgrounds = sprites;
        }

        if (westNeighbor != null)
        {
            List<Tile> neighbors = new List<Tile>();
            List<Sprite> sprites = new List<Sprite>(); 

            for (int i = 0; i < westNeighbor.tileSet.Tiles.Count - 1; i++)
            {
                if (westNeighbor.tileSet.Tiles[i].EAST == tile.WEST)
                {
                    neighbors.Add(westNeighbor.tileSet.Tiles[i]);
                    sprites.Add(westNeighbor.backgrounds[i]);
                }
            }
            westNeighbor.tileSet.Tiles = neighbors;
            westNeighbor.backgrounds = sprites;
        }
    }
}