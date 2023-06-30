using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CurrentTile : Cell
{
    public bool startTile;
    public bool failed;
    public Tile tile = new Tile();
    public TileSet tileSet = new TileSet();
    public List<Socket> sockets = new List<Socket>();
    public Sprite initSprite;
    public List<CurrentTile> neighborTiles = new List<CurrentTile>(4);

    public List<Sprite> backgrounds = new List<Sprite>();
    public SpriteRenderer spriteRenderer;

    int NE = 1; int SE = 2; int SW = 3; int NW = 4;
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
        SetNeighbors();

        if (startTile)
        {
            StartCoroutine(WaveFunction());
        }
    }
    private void RunCoroutine()
    {
        StartCoroutine(WaveFunction());
    }

    public IEnumerator WaveFunction()
    {
        if (!collapsed)
        {
            CollapseCommand collapse = new CollapseCommand(this, CommandManager, GridManager);
            collapse.Execute();

            yield return null;

            EleminateCommand eleminate = new EleminateCommand(this, CommandManager, GridManager);
            eleminate.Execute();

            yield return null;

            // WestToEastWave();

            //SpiralWave();

            EntropyWave();

            yield return null;
        }

        collapsed = true;
        yield return null;

    }
    void EntropyWave()
    {
        List<int> entropyAmounts = new List<int>();
        Debug.Log(index);

        foreach (var item in neighborTiles)
        {
            entropyAmounts.Add(item.tileSet.Tiles.Count);
        }
    
        int smallestEntropy = Mathf.Min(entropyAmounts.ToArray());

        CurrentTile collapsibleTile = neighborTiles.Find(f => f.tileSet.Tiles.Count == smallestEntropy);
        collapsibleTile.OnCollapsed();
    }
    void SpiralWave()
        {
            WaveFunctionManager.waveDirection = tileDirection();

            if (tileDirection() == Direction.NORTH)
            {
                neighborTiles[0].OnCollapsed();
            }
            else if (tileDirection() == Direction.EAST)
            {
                neighborTiles[1].OnCollapsed();
            }
            else if (tileDirection() == Direction.SOUTH)
            {
                neighborTiles[2].OnCollapsed();
            } 
            else if(tileDirection() == Direction.WEST)
            {
                neighborTiles[3].OnCollapsed();
            }
        }
    void WestToEastWave()
    {
        if (!GridManager.grid[index].collapsed)
        {
            GridManager.grid[index].OnCollapsed();
        }
    }
    Direction tileDirection()
    {
        // North West or West        
        if ((GetCornerContacts() == SW ^ GetContacts()[3]) && !GetContacts()[0]) return Direction.NORTH;

        // North East or North
        else if ((GetCornerContacts() == NW ^ GetContacts()[0]) && !GetContacts()[1] ) return Direction.EAST;
        
        // North East or South
        else if ((GetCornerContacts() == NW ^ GetContacts()[1]) && !GetContacts()[2] ) return Direction.SOUTH;

        // North East or South
        else if ((GetCornerContacts() == SE ^ GetContacts()[2]) && !GetContacts()[3] ) return Direction.WEST;
        Debug.Log(string.Join(", ", GetContacts().ToArray()));

        return Direction.NONE;
    }
    List<bool> GetContacts()
    {
        bool northCollapse = neighborTiles[0] == null || neighborTiles[0].collapsed;
        bool eastCollapse = neighborTiles[1] == null || neighborTiles[1].collapsed;
        bool southCollapse = neighborTiles[2] == null || neighborTiles[2].collapsed;
        bool westCollapse = neighborTiles[3] == null || neighborTiles[3].collapsed;

        return new List<bool>() { northCollapse, eastCollapse, southCollapse, westCollapse };
    }
    int GetCornerContacts()
    {
        if (GetContacts() == new List<bool>(){true, true, false, false})
        {
            return NE;
        }
        else if (GetContacts() == new List<bool>(){false, true, true, false})
        {
            return SE;
        }
        else if (GetContacts() == new List<bool>(){false, false, true, true})
        {
            return SW;
        }
        else if (GetContacts() == new List<bool>(){true, false, false, true})
        {
            return NW;
        }
        return 0;
    }
    void SetNeighbors()
    {
        List<CurrentTile> grid = GridManager.grid;
        neighborTiles.Capacity = 4;
        neighborTiles.AddRange(new List<CurrentTile>() { this, this, this, this });

        bool NorthNeighborExists()
        {
            return !(Mathf.Ceil(index / GridManager.columnNumber) == GridManager.columnNumber - 1 || index == GridManager.columnNumber * GridManager.rowNumber);
        }
        bool EastNeighborExists()
        {
            return !(index % GridManager.columnNumber == 0);
        }
        bool SouthNeighborExists()
        {
            return !(Mathf.Ceil(index / GridManager.columnNumber) == 0 || index == GridManager.columnNumber);
        }        
        bool WestNeighborExists()
        {
            return !(index % GridManager.columnNumber == 1);
        }

        if (NorthNeighborExists())
        {
            neighborTiles[0] = (grid[(index - 1) + GridManager.columnNumber]);
        }
        if (EastNeighborExists())
        {
            neighborTiles[1] = grid[(index - 1) + 1];            
        }
        if (SouthNeighborExists())
        {
            neighborTiles[2] = grid[(index - 1) - GridManager.columnNumber];            
        }
        if (WestNeighborExists())
        {
            neighborTiles[3] = grid[(index - 1) - 1];
        }   
    }  
    public void RollForTile(int random)
    {
        spriteRenderer.sprite = backgrounds[random];
        tile = tileSet.Tiles[random];

        sockets = new List<Socket>() { tile.NORTH, tile.EAST, tile.SOUTH, tile.WEST };
        return;
    }
    public void RemoveNeighborsFromLists()
    {
        if (neighborTiles[0] != null) 
        {
            List<Tile> neighbors = new List<Tile>();
            List<Sprite> sprites = new List<Sprite>();

            for (int i = 0; i < neighborTiles[0].tileSet.Tiles.Count; i++)
            {   
                if (tile.NORTH == neighborTiles[0].tileSet.Tiles[i].SOUTH)
                {
                    neighbors.Add(neighborTiles[0].tileSet.Tiles[i]);
                    sprites.Add(neighborTiles[0].backgrounds[i]);
                }
            }
            neighborTiles[0].tileSet.Tiles = neighbors;
            neighborTiles[0].backgrounds = sprites;
        }

        if (neighborTiles[1] != null)
        {
            List<Tile> neighbors = new List<Tile>();
            List<Sprite> sprites = new List<Sprite>(); 

            for (int i = 0; i < neighborTiles[1].tileSet.Tiles.Count; i++)
            {
                if (neighborTiles[1].tileSet.Tiles[i].WEST == tile.EAST)
                {
                    neighbors.Add(neighborTiles[1].tileSet.Tiles[i]);
                    sprites.Add(neighborTiles[1].backgrounds[i]);
                }
            }
            neighborTiles[1].tileSet.Tiles = neighbors;
            neighborTiles[1].backgrounds = sprites;
        }

        if (neighborTiles[2] != null)
        {
            List<Tile> neighbors = new List<Tile>(16);
            List<Sprite> sprites = new List<Sprite>(16);   


            for (int i = 0; i < neighborTiles[2].tileSet.Tiles.Count; i++)
            {
                if (neighborTiles[2].tileSet.Tiles[i].NORTH == tile.SOUTH)
                {
                    neighbors.Add(neighborTiles[2].tileSet.Tiles[i]);
                    sprites.Add(neighborTiles[2].backgrounds[i]);
                }
            }
            neighborTiles[2].tileSet.Tiles = neighbors;
            neighborTiles[2].backgrounds = sprites;
        }

        if (neighborTiles[3] != null)
        {
            List<Tile> neighbors = new List<Tile>();
            List<Sprite> sprites = new List<Sprite>(); 

            for (int i = 0; i < neighborTiles[3].tileSet.Tiles.Count; i++)
            {
                if (neighborTiles[3].tileSet.Tiles[i].EAST == tile.WEST)
                {
                    neighbors.Add(neighborTiles[3].tileSet.Tiles[i]);
                    sprites.Add(neighborTiles[3].backgrounds[i]);
                }
            }
            neighborTiles[3].tileSet.Tiles = neighbors;
            neighborTiles[3].backgrounds = sprites;
        }
    }
}