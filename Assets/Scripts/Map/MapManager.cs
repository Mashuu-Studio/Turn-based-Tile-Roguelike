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

    // 주변 방 상태에 따라서 맵을 골라서 랜덤으로 받아옴.
    // 우선은 아예 랜덤으로 받아옴.
    public static Map GetMap()
    {
        int rand = Random.Range(0, maps.Length);
        return maps[rand];
    }
}
