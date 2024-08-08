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
        // �켱 ��ü ���� Node�� ����        
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
        /* ���� ���´� 0, 1�� ����. 0�� ��, 1�� ��(�Ʊ� ����)
        * �������� �޾ƿ� �� ���� ��ġ�� ��(�Ʊ� ����)�� Ÿ�ٵ��� ��ġ�� üũ�ϰ�
        * A* Path�� Ȱ���ؼ� ���������� ������ ����ġ�� ����� ��
        * Ÿ�ٿ� ��� �Ǹ� �ش� Ÿ�ٱ��� ���� ���� ����
        * 
        * Ž���� �� ���� ������ ���� Ÿ���� ��Ҵ��� üũ�Ͽ� ���� �������� �����.
        * �� ��庰�� Ÿ���� ��ġ������ �Ÿ��� �缭 H���� �������ְ� ����.
        * 
        * ��������� ���� ������ �������������� ������������ ��������� �Ǿ�����
        * �̵��Ҷ����� �������� �̵��ؾ��ϹǷ� stack���� ����
        */

        // �ʿ� H�� ��������.
        // �� ��� �� H�� Ÿ�ٱ����� �Ÿ��� ���� �޶����� �Ǵµ�
        // �� ����� ���� Ÿ���� �������� H�� ������.

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

            // Ÿ�� �� �ϳ��� �����ߴٸ� �ش� �������� ���� ��ȯ
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
                // �� ���̰ų� ���̰ų� closedList�� �ִٸ� ��ŵ
                if (neighberPos.x < 0 || neighberPos.x >= map.GetLength(0)
                    || neighberPos.y < 0 || neighberPos.y >= map.GetLength(1)
                    || info[neighberPos.x, neighberPos.y] == WALL
                    || closedList.Contains(map[neighberPos.x, neighberPos.y]))
                {
                    continue;
                }

                AstarNode neighberNode = map[neighberPos.x, neighberPos.y];
                int moveCost = curNode.G + 1; // �����¿� �̵��� ����Ǳ� ������ ������ 1

                // �ڽ�Ʈ�� �� ���ų� ���� �� ���� �� ���� ��쿡 ������Ʈ
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
        // ���� ���� �ִٸ� ������ ���̱� ������ h�� 0���� ����
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