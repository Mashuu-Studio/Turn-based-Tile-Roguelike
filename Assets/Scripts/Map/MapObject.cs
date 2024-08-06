using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapObject : MonoBehaviour
{
    public Tilemap tilemap;
    public void SetMap(Map map)
    {
        Sprite[] sprites = new Sprite[3];
        sprites[0] = Resources.Load<Sprite>("NONE");
        sprites[1] = Resources.Load<Sprite>("FLOOR");
        sprites[2] = Resources.Load<Sprite>("OBSTACLE");

        var pos = Vector3Int.zero;
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                UnityEngine.Tilemaps.Tile tile = ScriptableObject.CreateInstance(typeof(UnityEngine.Tilemaps.Tile)) as UnityEngine.Tilemaps.Tile;
                tile.sprite = sprites[(int)map.Tiles[x, y].type];
                
                pos.x = x;
                pos.y = map.height - y; // 타일맵에서는 역순으로 나타나므로 역순으로 조정
                tilemap.SetTile(pos, tile);
            }
        }
    }

    public static MapObject Create(Map map)
    {
        map.DeserializeTiles();
        var go = new GameObject(map.name);
        var mapObject = go.AddComponent<MapObject>();
        var child = new GameObject("Tilemap");
        mapObject.tilemap = child.AddComponent<Tilemap>();
        child.AddComponent<TilemapRenderer>();
        child.transform.parent = mapObject.transform;

        mapObject.SetMap(map);

        return mapObject;
    }
}
