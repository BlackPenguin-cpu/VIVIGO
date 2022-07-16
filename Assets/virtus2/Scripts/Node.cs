using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    [Header("커스텀 변수")]
    public TILE_TYPE Type;
    public bool PlayerSpawn;
    public Vector3Int LocalPosition { get; set; }
    public Vector3 WorldPosition { get; set; }

    public string Name { get; set; }

    // Below is needed for Breadth First Searching
    public bool IsExplored { get; set; }

    public Node ExploredFrom { get; set; }

    public int Cost { get; set; }

}
