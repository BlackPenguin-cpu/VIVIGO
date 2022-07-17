using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector3 = UnityEngine.Vector3;


public class WorldTilemap : MonoBehaviour
{
    public Tilemap tilemap;
    public Dictionary<Vector3, Node> nodes;

    public Node StartNode;
    public Node GoalNode;
    public List<Node> ObjectNodes;
    public List<Node> KeyNodes;
    public List<Node> LockNodes;
    public List<Node> ObstacleNodes;
    public List<Node> MonsterNodes;
    private void Start()
    {
        int i = 0;
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
            node.EnemyType = tile.EnemyType;
            node.PlayerSpawn = tile.PlayerSpawn;
            node.Type = tile.Type;
            nodes.Add(node.WorldPosition, node);
            
            
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
        
        Debug.Log(nodes.Count);
        PathfindManager._Instance.SetNodes(StartNode, GoalNode);
        SpawnObjects();
    }

    public void SpawnObjects()
    {
        foreach (var pair in nodes)
        {
            var node = pair.Value;
            if (node.PlayerSpawn)
            {
                GameManager._Instance.CreatePlayer(node.WorldPosition);
                StartNode = node;
            }

            switch (node.Type)
            {
                case TILE_TYPE.GOAL:
                    GoalNode = node;
                    GameManager._Instance.CreateGrandma(node.WorldPosition);
                    break;
                case TILE_TYPE.KEY:
                    GameManager._Instance.CreateKey(node.WorldPosition);
                    //KeyNodes.Add(node);
                    break;
                case TILE_TYPE.LOCK:
                    GameManager._Instance.CreateLock(node.WorldPosition);
                    //LockNodes.Add(node);
                    break;
                case TILE_TYPE.OBSTACLE:
                    GameManager._Instance.CreateObstacle(node.WorldPosition);
                    break;
                default:
                    break;
            }

            if (node.EnemyType != ENEMY_TYPE.NONE)
            {
                GameManager._Instance.CreateMonster(node.WorldPosition, node.EnemyType);
            }
        }
    }
    
}
