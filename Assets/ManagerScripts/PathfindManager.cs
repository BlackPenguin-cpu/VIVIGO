using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfindManager : SingletonManager<PathfindManager>
{
    public Node StartNode;
    public Node EndNode;

    public const float XSize = 0.9f;
    public const float YSize = 0.7f; 

    public void SetNodes(Node StartNode, Node EndNode)
    {
        this.StartNode = StartNode;
        this.EndNode = EndNode;
    }
    private readonly Vector3[] neighborPositions =
    {
        //new Vector3(0.9f, 0,0),
        //new Vector3(0, 0.7f, 0),
        //new Vector3(-0.9f, 0, 0),
        //new Vector3(0, -0.7f, 0),

        Vector3.up,
        Vector3.right,
        Vector3.down,
        Vector3.left,
    
        // 대각선
        //Vector3Int.up + Vector3Int.right,
        //Vector3Int.up + Vector3Int.left,
        //Vector3Int.down + Vector3Int.right,
        //Vector3Int.down + Vector3Int.left
    };

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // var list = GetNeighborNodes(new Vector3(-2.5f, -1f, 0));
            // Debug.Log(list.Count);
            var path = GetPath(StartNode.WorldPosition, EndNode.WorldPosition);
            if (path != null)
            {

                Debug.Log(path.Count);
                for (int i = 1; i < path.Count; i++)
                {
                    Debug.Log(i + ": " + path[i].WorldPosition);
                    Debug.DrawLine(path[i-1].WorldPosition, path[i].WorldPosition, Color.red, 5.0f);
                }
            }
            
        }
    }



    /// <summary>
    /// WorldPosition 좌표의 타일이 플레이어가 이동 가능한 타일일 경우 true를 반환
    /// </summary>
    /// <param name="WorldPosition"></param>
    /// <returns></returns>
    public bool CanMove(Vector3 WorldPosition)
    {
        Tilemap tilemap = GameStage.Instance.GetCurrentTilemap().tilemap;

        bool success = true;
        Vector3Int cellPos = tilemap.WorldToCell(WorldPosition);
        var dstTile = tilemap.GetTile<CustomTile>(cellPos);
        // 이동하려는 좌표가 타일맵 범위 밖일 경우 실패
        if (dstTile == null) return false;
        // 장애물일때
        if (dstTile.Type == TILE_TYPE.OBSTACLE)
        {
            success = false;
        }


        return success;
    }

    /// <summary>
    /// WorldPosition좌표의 타일이 Goal 타일일 경우 true, 이외에는 false를 반환
    /// </summary>
    /// <param name="WorldPosition"></param>
    /// <returns></returns>
    public bool ReachedGoal(Vector3 WorldPosition)
    {
        Tilemap tilemap = GameStage.Instance.GetCurrentTilemap().tilemap;

        bool reached = false;
        Vector3Int cellPos = tilemap.WorldToCell(WorldPosition);
        var dstTile = tilemap.GetTile<CustomTile>(cellPos);
        if (dstTile == null) return false;

        if (dstTile.Type == TILE_TYPE.GOAL)
            reached = true;

        return reached;
    }
    
    public List<Node> GetPath(Vector3 src, Vector3 dst)
    {
        List<Node> path = new List<Node>();
        WorldTilemap worldTilemap = GameStage.Instance.GetCurrentTilemap();

        ClearExploredTiles();

        Queue<Node> queue = new Queue<Node>();
        var start = worldTilemap.nodes[src];
        var dest = worldTilemap.nodes[dst];
        queue.Enqueue(start);
        start.IsExplored = true;
        start.ExploredFrom = start;
        start.Cost = 0;
        bool reached = false;
        while (queue.Count > 0)
        {
            Node currentTile = queue.Dequeue();
            if (currentTile.WorldPosition == dest.WorldPosition)
            {
                reached = true;
                break;
            }
            Debug.Log("현재 타일 위치: " + currentTile.WorldPosition);
            // 상 하 좌 우 
            List<Node> neighbors = GetNeighborNodes(currentTile.WorldPosition);
            Debug.Log("이웃 타일 개수: " + neighbors.Count);
            for (int i = 0; i < neighbors.Count; i++)
            {
                Debug.Log("이웃 타일 위치: " + neighbors[i].WorldPosition);
                if (neighbors[i].IsExplored || neighbors[i].Type == TILE_TYPE.OBSTACLE) continue;
                neighbors[i].Cost = currentTile.Cost + 1;
                neighbors[i].ExploredFrom = currentTile;
                neighbors[i].IsExplored = true;
                queue.Enqueue(neighbors[i]);
            }
        }

        if (reached)
        {
            Debug.Log("탐색 성공");
            var current = dest;
            while (current.WorldPosition != start.WorldPosition)
            {
                path.Add(current);
                current = current.ExploredFrom;
            }

            Debug.Log(path.Count);
            path.Add(start);
            path.Reverse();
            return path;
        }
        else
        {
            Debug.Log("탐색 실패");
            return null;
        }
    }
    
    /// <summary>
    /// 상하좌우 이웃한 타일을 구한다
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <returns></returns>
    public List<Node> GetNeighborNodes(Vector3 currentPosition)
    {
        List<Node> neighbors = new List<Node>();
        WorldTilemap worldTilemap = GameStage.Instance.GetCurrentTilemap();
        foreach (var nextPositions in neighborPositions)
        {
            //var scaledPosition = new Vector3(nextPositions.x * XSize, nextPositions.y * YSize, 0);
            var position = currentPosition + nextPositions;
            Debug.Log(position.x + ", " + position.y + ", " + position.z);
            if (!worldTilemap.nodes.ContainsKey(position)) continue;
            
            Debug.Log(position.x + ", " + position.y + "좌표의 타일을 리스트에 넣는다.");
            Node neighbor = worldTilemap.nodes[position]; 
            Debug.Log(neighbor.WorldPosition + "를 넣었다.");


            neighbors.Add(neighbor);
        }
        return neighbors;
    }

    public void ClearExploredTiles()
    {
        WorldTilemap worldTilemap = GameStage.Instance.GetCurrentTilemap();
        var dict = worldTilemap.nodes;
        foreach (var tile in dict)
        {
            tile.Value.IsExplored = false;
        }
    }
}
