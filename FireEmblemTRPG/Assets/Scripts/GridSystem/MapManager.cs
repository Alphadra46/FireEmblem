using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    
    public OverlayTile overlayTilePrefab;
    public GameObject overlayContainer;

    public Dictionary<Vector2Int, OverlayTile> map;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        InitTilemap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public List<OverlayTile> GetNeighbourTiles(OverlayTile currentOverlayTile)
    {
        var map = MapManager.instance.map;

        List<OverlayTile> neighbours = new List<OverlayTile>();

        //Top Neighbour
        Vector2Int locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y+1);

        if (map.ContainsKey(locationToCheck))
        {
            if (Mathf.Abs(currentOverlayTile.gridLocation.z - map[locationToCheck].gridLocation.z) <= 1)
                neighbours.Add(map[locationToCheck]);
        }
        
        //Bottom Neighbour
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y-1);

        if (map.ContainsKey(locationToCheck))
        {
            if (Mathf.Abs(currentOverlayTile.gridLocation.z - map[locationToCheck].gridLocation.z) <= 1)
                neighbours.Add(map[locationToCheck]);
            
        }
        
        //Right Neighbour
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x+1, currentOverlayTile.gridLocation.y);

        if (map.ContainsKey(locationToCheck))
        {
            if (Mathf.Abs(currentOverlayTile.gridLocation.z - map[locationToCheck].gridLocation.z) <= 1)
                neighbours.Add(map[locationToCheck]);
        }
        
        //Left Neighbour
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x-1, currentOverlayTile.gridLocation.y);

        if (map.ContainsKey(locationToCheck))
        {
            if (Mathf.Abs(currentOverlayTile.gridLocation.z - map[locationToCheck].gridLocation.z) <= 1)
                neighbours.Add(map[locationToCheck]);
        }

        return neighbours;
    }
    private void InitTilemap()
    {
        var tileMap = GetComponentInChildren<UnityEngine.Tilemaps.Tilemap>();
        map = new Dictionary<Vector2Int, OverlayTile>();
        
        BoundsInt bounds = tileMap.cellBounds;

        //Looping through every tile on the map
        for (int z = bounds.min.z; z < bounds.max.z; z++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    var tileLocation = new Vector3Int(x, y, z);
                    var tileKey = new Vector2Int(x, y);

                    if (tileMap.HasTile(tileLocation) && !map.ContainsKey(tileKey))
                    {
                        var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
                        var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

                        overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z); //See if +1 on z is necessary
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder+1;
                        overlayTile.gridLocation = tileLocation;
                        map.Add(tileKey, overlayTile);
                    }
                }
            }
        }
    }
}
