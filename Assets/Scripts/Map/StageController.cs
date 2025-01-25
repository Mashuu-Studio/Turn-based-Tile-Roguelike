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
    // 활성화 된 맵 정보
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

        // 기본적으로 특정 값을 기본으로 진행
        // 방 갯수는 특정 값에 오차범위 방 2개
        // 맵 넓이는 (value / 2) * 2 + 1(시작지점) 홀수로 맞추기 위함.
        // 시작지점은 value / 2 , value / 2
        int value = (int)(level * 2.5f) + 8;
        int roomAmount = value + Random.Range(0, 3);
        int roomsize = (value / 2) * 2 + 1;
        width = height = roomsize;
        stage = new int[roomsize, roomsize];
        stage[value / 2, value / 2] = ROOM;
        roomAmount -= 1;

        // 좌 우 상 하 순으로 방 생성시도.
        Vector2Int[] directions = { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
        Queue<Vector2Int> q = new Queue<Vector2Int>();
        // 시작 지점 추가.
        q.Enqueue(new Vector2Int(value / 2, value / 2));
        while (roomAmount > 0)
        {
            // 큐가 남아있으면 우선 진행.
            while (q.Count > 0 && roomAmount > 0)
            {
                // 현재 있는 방들을 큐에 추가
                int size = q.Count;
                for (int i = 0; i < size && roomAmount > 0; i++)
                {
                    Vector2Int pos = q.Dequeue();

                    foreach (var dir in directions)
                    {
                        int x = dir.x + pos.x;
                        int y = dir.y + pos.y;
                        // 공간이 있으면 방 생성 시도. 성공하면 큐에 추가.
                        if (Available(x, y) && TryToCreateRoom(x, y, ref roomAmount))
                            q.Enqueue(new Vector2Int(x, y));
                    }
                }
            }

            // 만약에 큐를 다 비웠는데 방의 갯수가 부족하다면 다시 시작.
            // 모든 방을 큐에 추가해줌. 이 때 끝단부터 채우기 시작함.
            // roomsize는 기본적으로 홀수. 절반을 하면 중심점이 됨.
            for (int line = 0; line <= value / 2; line++)
            {
                for (int x = line; x < roomsize - line; x++)
                {
                    for (int y = line; y < roomsize - line; y++)
                    {
                        // 바깥 라인이고 방이면 큐에 추가.
                        if ((x == line || y == line
                            || x == roomsize - 1 - line || y == roomsize - 1 - line)
                            && stage[x, y] == ROOM)
                            q.Enqueue(new Vector2Int(x, y));
                    }
                }
            }
        }

        // 구성이 끝나고 나면 맵 배치. 우선은 한 종류로 세팅.
        // 이 때 시작지점은 방이 정해져있음. Start. 해당 맵으로 먼저 설정.
        // 방 별로 어디 입구가 비어있는지 미리 체크한 뒤
        // 방의 위치에 따라서 맵 선택 및 생성.
        // 맵끼리 떨어진 칸은 3칸. 벽 한칸, 여유공간 한 칸.
        // 일단은 맵의 크기를 11 * 7로 고정된다고 가정. 실제로 한 칸을 11 * 7로 사용할 예정.
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
                // 시작포인트면 시작지점맵으로 설정.
                if (start) SetMap(new Vector3Int(x, y), mapObject);
            }
        }
        PlayerController.Instance.SetPlayer(CurrentMap.RoomStartPos(Vector3Int.zero));
    }

    private int DoorInfo(int x, int y)
    {
        // 좌 우 상 하
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
        // -1: 이동 불가. 0: 방 내에서 이동(필요한 작동 X) 1: 다른 방으로 이동
        int value = CurrentMap.Move(pos.x + dir.x, pos.y + dir.y);

        if (value == 1)
        {
            return MoveMap(dir);
        }
        else return false;
    }

    // 다른 맵으로 이동.
    private bool MoveMap(Vector3Int dir)
    {
        Vector3Int pos = currentPos + dir;
        // 우선은 바로 이동 가능. 이 후에는 맵의 클리어 정도 등에 따라 이동 가능 불가능이 달라질 예정.
        if (Available(pos.x, pos.y) && maps.ContainsKey(pos))
        {
            SetMap(pos, maps[pos]);
            PlayerController.Instance.SetPlayer(CurrentMap.RoomStartPos(dir));
            return true;
        }
        return false; // 방이 없을 때는 이동할 수 없음.
    }
}
