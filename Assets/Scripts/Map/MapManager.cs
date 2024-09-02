using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MapManager
{
    private static Map[] maps;
    private static Dictionary<int, List<Map>> mapInfoWithDoor;

    public static void Init()
    {
        maps = Resources.LoadAll<Map>("Maps");

        // ��� �������� ����� ���� �°� �� �߰�
        mapInfoWithDoor = new Dictionary<int, List<Map>>();
        for (int i = 0; i <= 15; i++)
        {
            mapInfoWithDoor[i] = new List<Map>();
        }

        foreach (Map map in maps)
        {
            if (map.name.Contains("Start")) continue; // �켱 Start�� �����ϸ� ��ŵ.  
            int door = map.DoorInfoBit;
            // �� ���¿� �°� �߰�
            mapInfoWithDoor[door].Add(map);
        }
    }

    // �ֺ� �� ���¿� ���� ���� ��� �������� �޾ƿ�.
    public static Map GetMap(int door)
    {
        List<Map> list = new List<Map>();

        // door ���¿� �°� �� ����
        // �� �� �̵��� �� �ִ� ������ �� �־ ����� �� �ִ� ����. ���� �ƴ϶� ���� ���� ��.
        // �̵��� �� �ִ� ������ �� �ִٴ� ���� �⺻ door���� 0�� �κ��� 1�� �� �� �ִٴ� �ǹ�.
        // �켱 ��� �κ��� 0���� Ȯ��, �� �� ��� ���տ� ���Ͽ� ����.
        // �ش� index�� ����� �� �� ������ ���� ������ ��Ʈ����ŷ�� �̿��� Ȱ��.
        // ���� ��� index�� ������ 3����� 000�� ������ �������� �� bit�� 1�� �ȴٸ� ����Ѵٴ� �ǹ�.
        // �̸� �̿��� ��� ����� ���� ����.

        List<int> allComb = new List<int>();
        List<int> zeroIndexes = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            if ((door & (1 << i)) == 0) zeroIndexes.Add(i);
        }


        for (int i = 0; i < (1 << zeroIndexes.Count); i++)
        {
            int info = door;
            for (int j = 0; j < zeroIndexes.Count; j++)
            {
                if ((i & (1 << j)) != 0) info |= (1 << zeroIndexes[j]);
            }
            allComb.Add(info);
        }

        foreach (var info in allComb)
        {
            list.AddRange(mapInfoWithDoor[info]);
        }

        int rand = Random.Range(0, list.Count);
        return list[rand];
    }

    public static Map GetStartMap()
    {
        return maps[0];
    }
}
