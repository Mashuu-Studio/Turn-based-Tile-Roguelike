using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

    public bool Available(Vector3Int pos)
    {
        return pos.x >= 0 && pos.x < Width
            && pos.y >= 0 && pos.y < Height
            && (data.Tiles[pos.x, pos.y].type == Tile.TileType.FLOOR
            || data.Tiles[pos.x, pos.y].type == Tile.TileType.UNIT);
    }

    public static MapObject Create(Map map, int doorInfo)
    {
        map.Deserialize();
        var go = new GameObject(map.name);
        var mapObject = go.AddComponent<MapObject>();
        var child = new GameObject("Tilemap");
        mapObject.tilemap = child.AddComponent<Tilemap>();
        mapObject.tilemap.transform.position -= new Vector3(0.5f, 0.5f);
        child.AddComponent<TilemapRenderer>();
        child.transform.parent = mapObject.transform;

        mapObject.SetMap(map, doorInfo);

        return mapObject;
    }

    public void SetMap(Map map, int doorInfo)
    {
        data = map;
        Sprite[] sprites = new Sprite[4];
        sprites[0] = Resources.Load<Sprite>("NONE");
        sprites[1] = Resources.Load<Sprite>("FLOOR");
        sprites[2] = Resources.Load<Sprite>("OBSTACLE");
        sprites[3] = Resources.Load<Sprite>("FLOOR");

        var pos = Vector3Int.zero;
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                UnityEngine.Tilemaps.Tile tile = ScriptableObject.CreateInstance(typeof(UnityEngine.Tilemaps.Tile)) as UnityEngine.Tilemaps.Tile;

                tile.sprite = sprites[(int)map.Tiles[x, y].type];

                pos.x = x;
                pos.y = y; // Ÿ�ϸʿ����� �������� ��Ÿ���Ƿ� �������� ����
                tilemap.SetTile(pos, tile);
                tilemap.SetTileFlags(pos, TileFlags.None);
            }
        }

        for (int x = -1; x <= map.width; x++)
        {
            if (x == map.width / 2) continue;
            UnityEngine.Tilemaps.Tile tile = ScriptableObject.CreateInstance(typeof(UnityEngine.Tilemaps.Tile)) as UnityEngine.Tilemaps.Tile;
            tile.sprite = sprites[0];

            pos.x = x;
            pos.y = -1;
            tilemap.SetTile(pos, tile);
            tilemap.SetTileFlags(pos, TileFlags.None);
            pos.y = map.height;
            tilemap.SetTile(pos, tile);
            tilemap.SetTileFlags(pos, TileFlags.None);
        }

        for (int y = 0; y < map.height; y++)
        {
            if (y == map.height / 2) continue;
            UnityEngine.Tilemaps.Tile tile = ScriptableObject.CreateInstance(typeof(UnityEngine.Tilemaps.Tile)) as UnityEngine.Tilemaps.Tile;
            tile.sprite = sprites[0];

            pos.x = -1;
            pos.y = y;
            tilemap.SetTile(pos, tile);
            tilemap.SetTileFlags(pos, TileFlags.None);
            pos.x = map.width;
            tilemap.SetTile(pos, tile);
            tilemap.SetTileFlags(pos, TileFlags.None);
        }

        // �� �� �� ��
        {
            UnityEngine.Tilemaps.Tile tile = ScriptableObject.CreateInstance(typeof(UnityEngine.Tilemaps.Tile)) as UnityEngine.Tilemaps.Tile;
            if ((doorInfo & 1) == 1) tile.sprite = sprites[1];
            else tile.sprite = sprites[0];
            tilemap.SetTile(new Vector3Int(map.width / 2, -1), tile);
            doorInfo >>= 1;
        }
        {
            UnityEngine.Tilemaps.Tile tile = ScriptableObject.CreateInstance(typeof(UnityEngine.Tilemaps.Tile)) as UnityEngine.Tilemaps.Tile;
            if ((doorInfo & 1) == 1) tile.sprite = sprites[1];
            else tile.sprite = sprites[0];
            tilemap.SetTile(new Vector3Int(map.width / 2, map.height), tile);
            doorInfo >>= 1;
        }
        {
            UnityEngine.Tilemaps.Tile tile = ScriptableObject.CreateInstance(typeof(UnityEngine.Tilemaps.Tile)) as UnityEngine.Tilemaps.Tile;
            if ((doorInfo & 1) == 1) tile.sprite = sprites[1];
            else tile.sprite = sprites[0];
            tilemap.SetTile(new Vector3Int(map.width, map.height / 2), tile);
            doorInfo >>= 1;
        }
        {
            UnityEngine.Tilemaps.Tile tile = ScriptableObject.CreateInstance(typeof(UnityEngine.Tilemaps.Tile)) as UnityEngine.Tilemaps.Tile;
            if ((doorInfo & 1) == 1) tile.sprite = sprites[1];
            else tile.sprite = sprites[0];
            tilemap.SetTile(new Vector3Int(-1, map.height / 2), tile);
        }
        units = new List<UnitObject>();
        foreach (var p in map.EnemiePoses)
        {
            var go = new GameObject(name);
            go.transform.parent = transform;

            var unit = go.AddComponent<EnemyObject>();
            unit.Summon(map.GetEnemy(p), p);

            units.Add(unit);
        }
    }

    public void DeadUnit(UnitObject unit)
    {
        units.Remove(unit);
    }

    public void Enter()
    {
        //Astar.SetMapSize(data.width, data.height);
        UnitController.Instance.AddUnits(units);
    }

    // ���� ǥ�� �Լ�. 
    // ���������� �ӽ�. �Ŀ� �ڵ� ������ ���鼭 ����
    List<Vector3Int> colorList = new List<Vector3Int>();

    public void PlayerRange(Vector3Int originPos, List<Vector3Int> range, Vector2Int viewDirection)
    {
        // right�� ��쿡�� x, y
        // left�� ��쿡�� -x, y
        // up�� ��쿡�� -y, x
        // down�� ��쿡�� y, -x

        foreach (var pos in range)
        {
            var newPos = pos;
            if (viewDirection == Vector2Int.left)
            {
                newPos.x *= -1;
            }
            else if (viewDirection == Vector2Int.up)
            {
                int tmp = newPos.x;
                newPos.x = -newPos.y;
                newPos.y = tmp;
            }
            else if (viewDirection == Vector2Int.down)
            {
                int tmp = newPos.x;
                newPos.x = newPos.y;
                newPos.y = -tmp;
            }
            var p = newPos + originPos;
            tilemap.SetColor(p, Color.green);
            colorList.Add(p);
        }
    }

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

    // -1: �̵� �Ұ� 0: �� ������ �̵� 1: �ٸ� ������ �̵�
    public int Move(int x, int y)
    {
        // x�� �߾��̰� y�� ���� �� Ȥ�� y�� �߾��̰� x�� ���� �� �ٸ� ������ �̵� ����.
        if (x == data.width / 2 && (y < 0 || y >= data.height)
            || (y == data.height / 2 && (x < 0 || x >= data.width))) return 1;
        // �� ���� ��쿡 ���̶�� �̵� �Ұ�
        else if (x < 0 || y < 0 || x >= data.width || y >= data.height) return -1;
        // �� �ܿ��� �� ������ �̵�
        else return 0;
    }

    public Vector3Int RoomStartPos(Vector3Int dir)
    {
        // �޹�������(�����ʿ���) �������� x: width - 1, y: height/2
        // ������������(���ʿ���) �������� x: 0, y: height/2
        // ����������(�Ʒ�����) �������� x: width/2 y: 0
        // �Ʒ���������(������) �������� x: width/2 y: height - 1

        Vector3Int pos = new Vector3Int(Width / 2, Height / 2);
        if (dir == Vector3Int.left)
        {
            pos.x = Width - 1;
            pos.y = Height / 2;
        }

        if (dir == Vector3Int.right)
        {
            pos.x = 0;
            pos.y = Height / 2;
        }

        if (dir == Vector3Int.up)
        {
            pos.x = Width / 2;
            pos.y = 0;
        }

        if (dir == Vector3Int.down)
        {
            pos.x = Width / 2;
            pos.y = Height - 1;
        }
        return pos;
    }
}
