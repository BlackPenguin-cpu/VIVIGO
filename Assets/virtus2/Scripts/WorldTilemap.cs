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
                var go = Instantiate(player);
                go.transform.localPosition = tilemap.GetCellCenterLocal(tile.LocalPosition);

            }

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
