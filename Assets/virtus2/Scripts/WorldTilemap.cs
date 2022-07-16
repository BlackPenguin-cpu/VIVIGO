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
        int i = 0;
        tiles = new Dictionary<Vector3, CustomTile>();
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            var localPos = new Vector3Int(pos.x, pos.y, pos.z);
            if (!tilemap.HasTile(localPos)) continue;
            // 타일 정보 설정
            var tile = tilemap.GetTile<CustomTile>(localPos);
            tile.LocalPosition = localPos;
            tile.WorldPosition = tilemap.GetCellCenterWorld(pos);
            tile.Tilemap = tilemap;
            tile.Name = localPos.x + ", " + localPos.y + ": " + tile.Type.ToString();
            tile.GameObject = tilemap.GetInstantiatedObject(localPos);
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
            }
            tiles.Add(tile.WorldPosition, tile);
            Debug.Log(i + ": " + tile.WorldPosition);
            i++;
        }

        Debug.Log(tiles.Count);
    }
}
