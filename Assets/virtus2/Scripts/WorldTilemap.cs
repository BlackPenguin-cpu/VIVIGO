using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTilemap : MonoBehaviour
{
    public Tilemap tilemap;
    public Dictionary<Vector3, CustomTile> tiles;

    private void Start()
    {
        tiles = new Dictionary<Vector3, CustomTile>();
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            var localPos = new Vector3Int(pos.x, pos.y, pos.z);
            if (!tilemap.HasTile(localPos)) continue;
            // 타일 정보 설정
            var tile = tilemap.GetTile<CustomTile>(localPos);
            tile.LocalPosition = localPos;
            tile.WorldPosition = tilemap.CellToWorld(localPos);
            tile.Tilemap = tilemap;
            tile.Name = localPos.x + ", " + localPos.y + ": " + tile.Type.ToString();
            tile.GameObject = tilemap.GetInstantiatedObject(localPos);


            Debug.Log(tile.Name);
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
                Debug.Log(GameManager.Instance);
                Debug.Log(tilemap);
                Debug.Log(tile);
                GameManager.Instance.CreatePlayer(tilemap.GetCellCenterWorld(tile.LocalPosition));
            }

        }

        bool test = CanMove(new Vector3(-0.5f, -0.5f, 0));
        Debug.Log("obstacle test: Can Move? " + test);
        test = CanMove(new Vector3(-2.5f, -0.5f, 0));
        Debug.Log("default test: Can Move? " + test);
        test = CanMove(new Vector3(-100f, -100f, 0));
        Debug.Log("out of bounds test: Can Move? " + test);
    }

    /// <summary>
    /// WorldPosition 좌표의 타일이 플레이어가 이동 가능한 타일일 경우 true를 반환
    /// </summary>
    /// <param name="WorldPosition"></param>
    /// <returns></returns>
    public bool CanMove(Vector3 WorldPosition)
    {
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
        bool reached = false;
        Vector3Int cellPos = tilemap.WorldToCell(WorldPosition);
        var dstTile = tilemap.GetTile<CustomTile>(cellPos);
        if (dstTile == null) return false;

        if (dstTile.Type == TILE_TYPE.GOAL)
            reached = true;

        return reached;
    }
}
