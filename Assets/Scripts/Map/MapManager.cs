using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MapManager
{
    private static Map[] maps;
    public static void Init()
    {
        maps = Resources.LoadAll<Map>("Maps");
    }

    // �ֺ� �� ���¿� ���� ���� ��� �������� �޾ƿ�.
    // �켱�� �ƿ� �������� �޾ƿ�.
    public static Map GetMap()
    {
        // �켱 ���� ������ ���� ���� ���е� �� �ֵ��� ����.
        // ���߿��� ��� ������ ���� �����ִ� ���� ���� ���е� ��.
        int rand = Random.Range(1, maps.Length);
        return maps[rand];
    }

    public static Map GetStartMap()
    {
        return maps[0];
    }
}
