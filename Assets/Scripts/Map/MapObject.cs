using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class MapObject : MonoBehaviour
{
    private List<UnitObject> units;
    public Tilemap tilemap;
    public int Width { get { return data.width; } }
    public int Height { get { return data.height; } }
    private Map data;

    public Vector3 center
    {
        get
        {
            Vector3 pos = tilemap.transform.position +
                new Vector3(data.width / 2f, data.height / 2f, Camera.main.transform.position.z);
            return pos;
        }
    }

    public static MapObject Create(Map map)
    {
        map.DeserializeTiles();
        var go = new GameObject(map.name);
        var mapObject = go.AddComponent<MapObject>();
        var child = new GameObject("Tilemap");
        mapObject.tilemap = child.AddComponent<Tilemap>();
        mapObject.tilemap.transform.position -= new Vector3(0.5f, 0.5f);
        child.AddComponent<TilemapRenderer>();
        child.transform.parent = mapObject.transform;

        mapObject.SetMap(map);

        return mapObject;
    }

    public void SetMap(Map map)
    {
        data = map;
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
                pos.y = map.height - 1 - y; // 타일맵에서는 역순으로 나타나므로 역순으로 조정
                tilemap.SetTile(pos, tile);
                tilemap.SetTileFlags(pos, TileFlags.None);
            }
        }

        // 우선 임시로 세팅. 후에는 Map에서 정보를 받아올 예정.
        units = new List<UnitObject>();
        string[] names = new string[] { "TEST1", "TEST2" };
        foreach (var name in names)
        {
            var go = new GameObject(name);
            go.transform.parent = transform;

            var unit = go.AddComponent<EnemyObject>();
            Vector3Int p = new Vector3Int(Random.Range(0, map.width), Random.Range(0, map.height));
            unit.Summon(UnitManager.GetUnit(name), p);

            units.Add(unit);
        }

        UnitController.Instance.AddUnits(units);
    }

    public void DeadUnit(UnitObject unit)
    {
        units.Remove(unit);
    }

    public void EnterTheMap()
    {
        Astar.SetMapSize(data.width, data.height);
    }

    // 범위 표기 함수. 
    // 마찬가지로 임시. 후에 코드 정리가 들어가면서 조정
    List<Vector3Int> colorList = new List<Vector3Int>();
    public void ShowRange(Vector3Int originPos, List<Vector3Int> range)
    {
        foreach (var pos in range)
        {
            var p = pos + originPos;
            if (colorList.Contains(p)) continue;
            tilemap.SetColor(p, Color.red);
            colorList.Add(p);
        }
    }

    public void ClearRange()
    {
        foreach (var pos in colorList)
        {
            tilemap.SetColor(pos, Color.white);
        }
        colorList.Clear();
    }
}
