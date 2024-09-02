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
        // 우선 시작 지점의 방이 따로 구분될 수 있도록 해줌.
        // 나중에는 모든 종류의 방이 열려있는 문에 따라서 구분될 것.
        int rand = Random.Range(1, maps.Length);
        return maps[rand];
    }

    public static Map GetStartMap()
    {
        return maps[0];
    }
}
