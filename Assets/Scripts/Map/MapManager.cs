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

        // 모든 방형태의 경우의 수에 맞게 방 추가
        mapInfoWithDoor = new Dictionary<int, List<Map>>();
        for (int i = 0; i <= 15; i++)
        {
            mapInfoWithDoor[i] = new List<Map>();
        }

        foreach (Map map in maps)
        {
            if (map.name.Contains("Start")) continue; // 우선 Start만 포함하면 스킵.  
            int door = map.DoorInfoBit;
            // 문 형태에 맞게 추가
            mapInfoWithDoor[door].Add(map);
        }
    }

    // 주변 방 상태에 따라서 맵을 골라서 랜덤으로 받아옴.
    public static Map GetMap(int door)
    {
        List<Map> list = new List<Map>();

        // door 형태에 맞게 방 선택
        // 이 때 이동할 수 있는 공간이 더 있어도 사용할 수 있는 방임. 문이 아니라 벽이 생길 뿐.
        // 이동할 수 있는 공간이 더 있다는 얘기는 기본 door에서 0인 부분이 1이 될 수 있다는 의미.
        // 우선 어느 부분이 0인지 확인, 이 후 모든 조합에 대하여 세팅.
        // 해당 index를 사용할 지 안 할지에 대한 정보를 비트마스킹을 이용해 활용.
        // 예를 들어 index의 갯수가 3개라면 000의 정보를 기준으로 각 bit가 1이 된다면 사용한다는 의미.
        // 이를 이용해 모든 경우의 수를 받음.

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
