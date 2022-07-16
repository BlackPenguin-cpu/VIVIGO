using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfindManager : SingletonManager<PathfindManager>
{
    private readonly Vector3Int[] neighborPositions =
    {
        Vector3Int.up,
        Vector3Int.right,
        Vector3Int.down,
        Vector3Int.left,
    
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
            var path = GetPath(new Vector3(-2.5f, -1f, 0), new Vector3(2.5f, 1f, 0));
            Debug.Log(path.Count);
            for (int i = 0; i < path.Count; i++)
            {
                path[i].GameObject.SetActive(false);
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

    public List<CustomTile> GetPath(Vector3 src, Vector3 dst)
    {
        List<CustomTile> path = new List<CustomTile>();
        WorldTilemap worldTilemap = GameStage.Instance.GetCurrentTilemap();

        ClearExploredTiles();

        Queue<CustomTile> queue = new Queue<CustomTile>();
        var startTile = worldTilemap.tiles[src];
        var dstTile = worldTilemap.tiles[dst];
        queue.Enqueue(startTile);
        startTile.IsExplored = true;
        startTile.ExploredFrom = startTile;
        startTile.Cost = 0;
        bool reached = false;
        while (queue.Count > 0)
        {
            CustomTile currentTile = queue.Dequeue();
            if (currentTile.WorldPosition == dstTile.WorldPosition)
            {
                reached = true;
                break;
            }
            Debug.Log("현재 타일 위치: " + currentTile.WorldPosition);
            // 상 하 좌 우 
            List<CustomTile> neighbors = GetNeighborTiles(currentTile.WorldPosition);
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
            var current = dstTile;
            while (current.WorldPosition != startTile.WorldPosition)
            {
                path.Add(current);
                current = current.ExploredFrom;
            }

            Debug.Log(path.Count);
            path.Add(startTile);
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
    public List<CustomTile> GetNeighborTiles(Vector3 currentPosition)
    {
        List<CustomTile> neighborTiles = new List<CustomTile>();
        WorldTilemap worldTilemap = GameStage.Instance.GetCurrentTilemap();
        
        foreach (var nextPositions in neighborPositions)
        {
            var position = currentPosition + nextPositions;
            Debug.Log(position);
            if (!worldTilemap.tiles.ContainsKey(position)) continue;
            var neighbor = worldTilemap.tiles[position];
            neighborTiles.Add(neighbor);
        }
        return neighborTiles;
    }

    public void ClearExploredTiles()
    {
        WorldTilemap worldTilemap = GameStage.Instance.GetCurrentTilemap();
        var dict = worldTilemap.tiles;
        foreach (var tile in dict)
        {
            tile.Value.IsExplored = false;
        }
    }
}
