using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Astar
{
    public const int ROAD = 0;
    public const int WALL = 1;
    private static AstarNode[,] map;
    private static int width, height;
    private static Vector2Int[] directions =
    {
        Vector2Int.left,
        Vector2Int.right,
        Vector2Int.up,
        Vector2Int.down,
    };

    public static void SetMapSize(int w, int h)
    {
        // 우선 전체 맵을 Node로 세팅        
        width = w;
        height = h;
        map = new AstarNode[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = new AstarNode(new Vector2Int(x, y));
            }
        }
    }
    /*
    public static Stack<Vector2Int> Search(int[,] info, Vector2Int start, Vector2Int target, List<Vector2Int> obstacles, int range)
    {
        /* 맵의 형태는 0, 1로 세팅. 0은 길, 1은 벽(아군 유닛)
        * 맵정보를 받아온 뒤 현재 위치랑 벽(아군 유닛)과 타겟들의 위치를 체크하고
        * A* Path를 활용해서 퍼져나가는 식으로 가중치를 계산한 뒤
        * 타겟에 닿게 되면 해당 타겟까지 길을 만들어서 리턴
        * 
        * 탐색할 때 공격 범위를 통해 타겟이 닿았는지 체크하여 최종 목적지를 잡아줌.
        * 각 노드별로 타겟의 위치까지의 거리를 재서 H값을 세팅해주고 진행.
        * 
        * 결과적으로 길의 순서는 목적지에서부터 시작지점까지 만들어지게 되어있음
        * 이동할때에는 역순으로 이동해야하므로 stack으로 관리
        */

        // 맵에 H를 세팅해줌.
        // 각 노드 별 H는 타겟까지의 거리에 따라 달라지게 되는데
        // 더 가까운 쪽의 타겟을 기준으로 H를 세팅함.

       /* for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (info[x, y] == 1) continue;
                var pos = new Vector2Int(x, y);
                int min = width * height;
                foreach (var target in targets)
                {
                    int h = CalculateH(pos, target, range);
                    if (min > h) min = h;
                }
                map[x, y].G = 0;
                map[x, y].H = min;
            }
        }

        List<AstarNode> openList = new List<AstarNode>() { map[start.x, start.y] };
        List<AstarNode> closedList = new List<AstarNode>();
        Stack<Vector2Int> path = new Stack<Vector2Int>();

        while (openList.Count > 0)
        {
            AstarNode curNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (curNode.F >= openList[i].F && curNode.H > openList[i].H)
                {
                    curNode = openList[i];
                }
            }

            // 타겟 중 하나에 도달했다면 해당 노드까지의 길을 반환
            foreach (var target in targets)
            {
                if (TargetInRange(curNode.pos, target, range))
                {
                    AstarNode pathNode = curNode;
                    while (pathNode != map[start.x, start.y])
                    {
                        path.Push(pathNode.pos);
                        pathNode = pathNode.parent;
                    }
                    return path;
                }
            }

            foreach (var dir in directions)
            {
                Vector2Int neighberPos = curNode.pos + dir;
                // 맵 밖이거나 벽이거나 closedList에 있다면 스킵
                if (neighberPos.x < 0 || neighberPos.x >= map.GetLength(0)
                    || neighberPos.y < 0 || neighberPos.y >= map.GetLength(1)
                    || info[neighberPos.x, neighberPos.y] == WALL
                    || closedList.Contains(map[neighberPos.x, neighberPos.y]))
                {
                    continue;
                }

                AstarNode neighberNode = map[neighberPos.x, neighberPos.y];
                int moveCost = curNode.G + 1; // 상하좌우 이동만 진행되기 때문에 무조건 1

                // 코스트가 더 적거나 아직 한 번도 안 했을 경우에 업데이트
                if (moveCost < neighberNode.G || !openList.Contains(neighberNode))
                {
                    neighberNode.G = moveCost;
                    neighberNode.parent = curNode;

                    openList.Add(neighberNode);
                }
            }

            openList.Remove(curNode);
            closedList.Add(curNode);
        }
        return path;
    }
       */
    private static int CalculateH(Vector2Int pos, Vector2Int target, int range)
    {
        var h = Mathf.Abs(pos.x - target.x) + Mathf.Abs(pos.y - target.y);
        // 범위 내에 있다면 도착한 것이기 때문에 h를 0으로 진행
        if (TargetInRange(pos, target, range)) h = 0;
        return h;
    }

    private static bool TargetInRange(Vector2Int pos1, Vector2Int pos2, int range)
    {
        return Mathf.Abs(pos1.x - pos2.x) <= range
            && Mathf.Abs(pos1.y - pos2.y) <= range;
    }

    public class AstarNode
    {
        public AstarNode parent;
        public int F { get { return G + H; } }
        public int G, H;
        public Vector2Int pos;

        public AstarNode(Vector2Int pos)
        {
            this.pos = pos;
        }
    }
}