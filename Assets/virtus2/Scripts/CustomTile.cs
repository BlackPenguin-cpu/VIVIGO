using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TILE_TYPE
{
    DEFAULT,
    OBSTACLE,
    MUD,
    ICE,
    LOCK
}
[CreateAssetMenu]
public class CustomTile : Tile
{
    [Header("커스텀 변수")]
    public TILE_TYPE Type;
    public bool PlayerSpawn;
    public Vector3Int LocalPosition { get; set; }
    public Vector3 WorldPosition { get; set; }
    public Tilemap Tilemap { get; set; }
    public TileData TileData { get; set; }
    public GameObject GameObject { get; set; }

    public string Name { get; set; }

    // Below is needed for Breadth First Searching
    public bool IsExplored { get; set; }

    public CustomTile ExploredFrom { get; set; }

    public int Cost { get; set; }
}
