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
    LOCK,
    KEY,
    GOAL,
}

public enum ENEMY_TYPE
{
    NONE,
    CAT,
    WOLF,
    RABBIT_WOLF
}
[CreateAssetMenu]
public class CustomTile : Tile
{
    [Header("커스텀 변수")]
    public TILE_TYPE Type;
    public bool PlayerSpawn;
    public ENEMY_TYPE EnemyType;
    public Vector3Int LocalPosition { get; set; }
    public Vector3 WorldPosition { get; set; }
    public Vector3 WorldCenterPosition { get; set; }
    public Tilemap Tilemap { get; set; }
    public TileData TileData { get; set; }

    public string Name { get; set; }

    // Below is needed for Breadth First Searching
    public bool IsExplored { get; set; }

    public CustomTile ExploredFrom { get; set; }

    public int Cost { get; set; }
}
