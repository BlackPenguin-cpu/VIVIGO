using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTilemap : MonoBehaviour
{
    public Player player;

    public Tilemap tilemap;
    public Dictionary<Vector3, CustomTile> tiles;

    private void Awake()
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
                GameManager.Instance.CreatePlayer(tilemap.GetCellCenterLocal(tile.LocalPosition));
            }

        }

        bool test = CanMove(new Vector3(-0.5f, -0.5f, 0));
        Debug.Log("obstacle test: Can Move? " + test);
        test = CanMove(new Vector3(-2.5f, -0.5f, 0));
        Debug.Log("default test: Can Move? " + test);
        test = CanMove(new Vector3(-100f, -100f, 0));
        Debug.Log("out of bounds test: Can Move? " + test);
    }

    public bool CanMove(Vector3 WorldPosition)
    {
        bool success = true;
        Vector3Int cellPos = tilemap.WorldToCell(WorldPosition);
        Vector3 movingPos = tilemap.GetCellCenterWorld(cellPos);

        var dstTile = tilemap.GetTile<CustomTile>(cellPos);
        if (dstTile == null)
        {
            // 범위 밖 
            return false;
        }

        if (dstTile.Type == TILE_TYPE.OBSTACLE)
        {
            // 장애물
            success = false;
        }


        return success;
    }
}
