using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Tilemaps;
using Vector3 = UnityEngine.Vector3;


public class WorldTilemap : MonoBehaviour
{
    public Tilemap tilemap;
    public Dictionary<Vector3, CustomTile> tiles;
    public Dictionary<Vector3, Node> nodes;

    public Node StartNode;
    public Node GoalNode;

    private void Start()
    {
        int i = 0;
        tiles = new Dictionary<Vector3, CustomTile>();
        nodes = new Dictionary<Vector3, Node>();
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            var localPos = new Vector3Int(pos.x, pos.y, pos.z);
            if (!tilemap.HasTile(localPos)) continue;
            // 타일 정보 설정
            var tile = tilemap.GetTile<CustomTile>(localPos);

            Node node = new Node();
            node.Type = tile.Type;
            node.LocalPosition = localPos;
            node.WorldPosition = tilemap.GetCellCenterWorld(pos);
            nodes.Add(node.WorldPosition, node);

            if (tile.PlayerSpawn)
            {
                GameManager.Instance.CreatePlayer(node.WorldPosition);
                StartNode = node;
            }
            else if (tile.Type == TILE_TYPE.GOAL)
            {
                GoalNode = node;
            }
            else if (tile.EnemyType != ENEMY_TYPE.NONE)
            {
                GameManager.Instance.CreateMonster(node.WorldPosition, tile.EnemyType);
            }
            i++;
            /*
            tile.LocalPosition = localPos;
            tile.WorldPosition = tilemap.CellToWorld(pos);
            tile.WorldCenterPosition = tilemap.GetCellCenterWorld(pos);
            tiles.Add(tile.WorldCenterPosition, tile);
            Debug.Log("Center: " + tile.WorldCenterPosition);
            tile.Tilemap = tilemap;
            tile.Name = localPos.x + ", " + localPos.y + ": " + tile.Type.ToString();
            switch (tile.Type)
            {
                case TILE_TYPE.DEFAULT:
                    break;
                case TILE_TYPE.OBSTACLE:
                    break;
                case TILE_TYPE.ICE:
                    break;
                default:
                    break;
            }

            if (tile.PlayerSpawn)
            {
                GameManager.Instance.CreatePlayer(tilemap.GetCellCenterWorld(tile.LocalPosition));
            }*/
        }
        
        foreach (var n in nodes)
        {
            Debug.Log(n.Key + "::" + n.Value.WorldPosition);
        }
        Debug.Log(nodes.Count);
        PathfindManager.Instance.SetNodes(StartNode, GoalNode);
    }

    private void Initialize()
    {
        tilemap.CompressBounds();
        var bounds = tilemap.cellBounds;
        var spots = new Vector3Int[bounds.size.x, bounds.size.y];

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                var px = bounds.xMin + x;
                var py = bounds.yMin + y;
                if (tilemap.HasTile(new Vector3Int(px, py, 0)))
                {
                    spots[x, y] = new Vector3Int(px, py, 0);
                }
                else
                {
                    spots[x, y] = new Vector3Int(px, py, 1);
                }
            }
        }
    }

    public CustomTile GetTileByWorldPosition(Vector3 worldPos)
    {
        var pos = tilemap.WorldToCell(worldPos);
        var tile = tilemap.GetTile<CustomTile>(pos);
        return tile;
    }
    
}
