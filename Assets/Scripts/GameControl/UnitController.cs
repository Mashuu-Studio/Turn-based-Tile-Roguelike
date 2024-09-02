using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    #region Instance
    public static UnitController Instance { get { return instance; } }
    private static UnitController instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    // 현재 맵의 정보. width, height 등.
    List<UnitObject> units;

    // 현재 맵의 유닛을 추가시킬 함수
    // 맵에서 접근.
    public void AddUnits(List<UnitObject> units)
    {
        this.units = units;
    }

    // 죽으면 삭제할 함수
    public void DeadUnit(UnitObject unit)
    {
        // 전부 죽으면 맵 오브젝트에서도 해당 유닛이 죽은걸 알려야 함.
        // 혹은 클리어 됐을 때 알리는 식.
    }

    // 턴을 진행시킬 함수 -> 전체 유닛이 순차적으로 행동해야함.
    // 플레이어가 행동을 진행하면 이어서 연달아 진행.
    Vector3Int[] dirs = new Vector3Int[4]
    {
        Vector3Int.left,
        Vector3Int.right,
        Vector3Int.up,
        Vector3Int.down
    };
    public void ActivateUnits()
    {
        StageController.Instance.CurrentMap.ClearRange();
        // 우선 랜덤으로 작동. 
        foreach (var unit in units)
        {
            var rand = Random.Range(0, 2);
            if (rand == 0)
            {
                // 랜덤 이동을 위해 세팅. 나중에는 Astar가 자리할 예정.
                Vector3Int dir;
                Vector3Int pos;
                do
                {
                    int r = Random.Range(0, dirs.Length);
                    dir = dirs[r];
                    pos = unit.Pos + dir;
                } while (StageController.Instance.CurrentMap.Available(pos));
                unit.Move(dir);
            }
            else
            {
                unit.Attack();
            }
        }
    }
}
