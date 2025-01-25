using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public static StageController Instance { get { return instance; } }
    private static StageController instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        gameObject.AddComponent<Grid>();
    }

    public int[,] stage;
    private int width, height;
    private Dictionary<Vector3Int, MapObject> maps = new Dictionary<Vector3Int, MapObject>();

    private Vector3Int currentPos = Vector3Int.zero;
    // Ȱ��ȭ �� �� ����
    public MapObject CurrentMap { get { return maps[currentPos]; ; } }

    public void SetMap(Vector3Int pos, MapObject map)
    {
        currentPos = pos;
        map.Enter();
        Camera.main.transform.position = CurrentMap.center;
    }

    private static int[] roomProb = { 50, 25, 10, 2 };
    private const int NONE = 0;
    private const int ROOM = 1;
    private const int BOSSROOM = 2;
    public void CreateStage(int level)
    {
        foreach (var obj in maps)
        {
            Destroy(obj.Value.gameObject);
        }
        maps.Clear();

        // �⺻������ Ư�� ���� �⺻���� ����
        // �� ������ Ư�� ���� �������� �� 2��
        // �� ���̴� (value / 2) * 2 + 1(��������) Ȧ���� ���߱� ����.
        // ���������� value / 2 , value / 2
        int value = (int)(level * 2.5f) + 8;
        int roomAmount = value + Random.Range(0, 3);
        int roomsize = (value / 2) * 2 + 1;
        width = height = roomsize;
        stage = new int[roomsize, roomsize];
        stage[value / 2, value / 2] = ROOM;
        roomAmount -= 1;

        // �� �� �� �� ������ �� �����õ�.
        Vector2Int[] directions = { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
        Queue<Vector2Int> q = new Queue<Vector2Int>();
        // ���� ���� �߰�.
        q.Enqueue(new Vector2Int(value / 2, value / 2));
        while (roomAmount > 0)
        {
            // ť�� ���������� �켱 ����.
            while (q.Count > 0 && roomAmount > 0)
            {
                // ���� �ִ� ����� ť�� �߰�
                int size = q.Count;
                for (int i = 0; i < size && roomAmount > 0; i++)
                {
                    Vector2Int pos = q.Dequeue();

                    foreach (var dir in directions)
                    {
                        int x = dir.x + pos.x;
                        int y = dir.y + pos.y;
                        // ������ ������ �� ���� �õ�. �����ϸ� ť�� �߰�.
                        if (Available(x, y) && TryToCreateRoom(x, y, ref roomAmount))
                            q.Enqueue(new Vector2Int(x, y));
                    }
                }
            }

            // ���࿡ ť�� �� ����µ� ���� ������ �����ϴٸ� �ٽ� ����.
            // ��� ���� ť�� �߰�����. �� �� ���ܺ��� ä��� ������.
            // roomsize�� �⺻������ Ȧ��. ������ �ϸ� �߽����� ��.
            for (int line = 0; line <= value / 2; line++)
            {
                for (int x = line; x < roomsize - line; x++)
                {
                    for (int y = line; y < roomsize - line; y++)
                    {
                        // �ٱ� �����̰� ���̸� ť�� �߰�.
                        if ((x == line || y == line
                            || x == roomsize - 1 - line || y == roomsize - 1 - line)
                            && stage[x, y] == ROOM)
                            q.Enqueue(new Vector2Int(x, y));
                    }
                }
            }
        }

        // ������ ������ ���� �� ��ġ. �켱�� �� ������ ����.
        // �� �� ���������� ���� ����������. Start. �ش� ������ ���� ����.
        // �� ���� ��� �Ա��� ����ִ��� �̸� üũ�� ��
        // ���� ��ġ�� ���� �� ���� �� ����.
        // �ʳ��� ������ ĭ�� 3ĭ. �� ��ĭ, �������� �� ĭ.
        // �ϴ��� ���� ũ�⸦ 11 * 7�� �����ȴٰ� ����. ������ �� ĭ�� 11 * 7�� ����� ����.
        for (int x = 0; x < roomsize; x++)
        {
            for (int y = 0; y < roomsize; y++)
            {
                if (stage[x, y] == NONE) continue;
                bool start = x == value / 2 && y == value / 2;
                var map = start ? MapManager.GetStartMap() : MapManager.GetMap(DoorInfo(x, y));
                var mapObject = MapObject.Create(map, DoorInfo(x, y));
                mapObject.transform.position = new Vector2(x * (11 + 3), y * (11 + 3));
                mapObject.transform.parent = transform;
                maps.Add(new Vector3Int(x, y), mapObject);
                // ��������Ʈ�� �������������� ����.
                if (start) SetMap(new Vector3Int(x, y), mapObject);
            }
        }
        PlayerController.Instance.SetPlayer(CurrentMap.RoomStartPos(Vector3Int.zero));
    }

    private int DoorInfo(int x, int y)
    {
        // �� �� �� ��
        int value = 0;
        if (Available(x - 1, y) && stage[x - 1, y] == ROOM) value += 1;
        value <<= 1;
        if (Available(x + 1, y) && stage[x + 1, y] == ROOM) value += 1;
        value <<= 1;
        if (Available(x, y + 1) && stage[x, y + 1] == ROOM) value += 1;
        value <<= 1;
        if (Available(x, y - 1) && stage[x, y - 1] == ROOM) value += 1;

        return value;
    }

    private bool Available(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    private bool TryToCreateRoom(int x, int y, ref int roomAmount)
    {
        int prob = roomProb[GetAroundRoom(x, y) - 1];
        bool b = Random.Range(0, 100) < prob;
        if (b)
        {
            stage[x, y] = ROOM;
            roomAmount--;
        }

        return b;
    }

    private int GetAroundRoom(int x, int y)
    {
        int count = 0;
        if (x > 1 && stage[x - 1, y] == ROOM) count++;
        if (x < width - 1 && stage[x + 1, y] == ROOM) count++;
        if (y > 1 && stage[x, y - 1] == ROOM) count++;
        if (y < height - 1 && stage[x, y + 1] == ROOM) count++;

        return count;
    }

    public bool Move(Vector3Int pos, Vector3Int dir)
    {
        // -1: �̵� �Ұ�. 0: �� ������ �̵�(�ʿ��� �۵� X) 1: �ٸ� ������ �̵�
        int value = CurrentMap.Move(pos.x + dir.x, pos.y + dir.y);

        if (value == 1)
        {
            return MoveMap(dir);
        }
        else return false;
    }

    // �ٸ� ������ �̵�.
    private bool MoveMap(Vector3Int dir)
    {
        Vector3Int pos = currentPos + dir;
        // �켱�� �ٷ� �̵� ����. �� �Ŀ��� ���� Ŭ���� ���� � ���� �̵� ���� �Ұ����� �޶��� ����.
        if (Available(pos.x, pos.y) && maps.ContainsKey(pos))
        {
            SetMap(pos, maps[pos]);
            PlayerController.Instance.SetPlayer(CurrentMap.RoomStartPos(dir));
            return true;
        }
        return false; // ���� ���� ���� �̵��� �� ����.
    }
}
