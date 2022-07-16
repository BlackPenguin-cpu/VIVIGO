using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfindManager : SingletonManager<PathfindManager>
{

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
}
