using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CurrentTile : MonoBehaviour
{
    TileSet tileNeighbors = new TileSet();

    [SerializeField] Sprite[] backgrounds;

    public float size;
    public List<Tile> northTileNeighbors = new List<Tile>();
    public CurrentTile northNeighbor;

    public List<Tile> eastTileNeighbors = new List<Tile>();
    public CurrentTile eastNeighbor;

    public List<Tile> southTileNeighbors = new List<Tile>();
    public CurrentTile southNeighbor;

    public List<Tile> westTileNeighbors = new List<Tile>();
    public CurrentTile westNeighbor;
    public bool collapsed;

    void Awake()
    {
        northTileNeighbors = tileNeighbors.Tiles;
        eastTileNeighbors = tileNeighbors.Tiles;
        southTileNeighbors = tileNeighbors.Tiles;
        westTileNeighbors = tileNeighbors.Tiles;
    }
    void Start()
    {
        NeighborChecks();
        // NeighborTileChanger();
        RollForTile();
    }
    void NeighborChecks()
    {
        List<Transform> transforms = new List<Transform>();

        RaycastHit2D raycastHitNorth = Physics2D.Raycast((Vector2)transform.position + Vector2.up, Vector2.up, .5f);
        if (raycastHitNorth.collider != null)
        {
            northNeighbor = raycastHitNorth.collider.gameObject.GetComponent<CurrentTile>();
        }

        RaycastHit2D raycastHitEast = Physics2D.Raycast((Vector2)transform.position + Vector2.right, Vector2.right, .5f);
        if (raycastHitEast.collider != null)
        {
            eastNeighbor = raycastHitEast.collider.gameObject.GetComponent<CurrentTile>(); 
        }

        RaycastHit2D raycastHitSouth = Physics2D.Raycast((Vector2)transform.position + Vector2.down, Vector2.down, .5f);
        if (raycastHitSouth.collider != null)
        {
            southNeighbor = raycastHitSouth.collider.gameObject.GetComponent<CurrentTile>();            
        }

        RaycastHit2D raycastHitWest = Physics2D.Raycast((Vector2)transform.position + Vector2.left, Vector2.left, .5f);        
        if (raycastHitWest.collider != null)
        {
            westNeighbor = raycastHitWest.collider.gameObject.GetComponent<CurrentTile>();            
        }
    }
    void NeighborTileChanger()
    {

    }
    void RollForTile()
    {
        int random = UnityEngine.Random.Range(0, 16);

        if (northNeighbor != null && !northNeighbor.collapsed)
        {
            int randomMax = northNeighbor.southTileNeighbors.Count+1;
            random = UnityEngine.Random.Range(0, randomMax);

            if (random < randomMax - 1)
            {
                GetComponent<SpriteRenderer>().sprite = backgrounds[random];
                collapsed = true;

                return;
            }
        }
        if (eastNeighbor != null && !eastNeighbor.collapsed)
        {
            int randomMax = eastNeighbor.westTileNeighbors.Count+1;
            random = UnityEngine.Random.Range(0, randomMax);

            if (random < randomMax - 1)
            {
                GetComponent<SpriteRenderer>().sprite = backgrounds[random];
                collapsed = true;

                return;
            }
        }
        if (southNeighbor != null && !southNeighbor.collapsed)
        {
            int randomMax = southNeighbor.northTileNeighbors.Count+1;
            random = UnityEngine.Random.Range(0, randomMax);

            if (random < randomMax - 1)
            {
                GetComponent<SpriteRenderer>().sprite = backgrounds[random];
                collapsed = true;

                return;
            }
        }
        if (westNeighbor != null && !westNeighbor.collapsed)
        {
            int randomMax = westNeighbor.eastTileNeighbors.Count+1;
            random = UnityEngine.Random.Range(0, randomMax);

            if (random < randomMax - 1)
            {
                GetComponent<SpriteRenderer>().sprite = backgrounds[random];
                collapsed = true;

                return;
            }
        }
        Debug.Log("failed");
        RollForTile();
    }

}
