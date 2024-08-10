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
        int rand = Random.Range(0, maps.Length);
        return maps[rand];
    }
}
