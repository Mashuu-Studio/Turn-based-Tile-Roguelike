using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : UnitObject
{
    public override void Attack()
    {
        // 특정 위치를 공격해야함.
        // 문제는 해당 정보를 어떻게 넘길지 고민.
        // 현재 활성화 된 맵을 알 수 있다면 좋을 듯.

        // 우선 임시로 현재 Player를 가지고 있는 GameController에 직접적으로 연결
        // 후에는 독립적으로 사용될 예정.

        StageController.Instance.CurrentMap.ShowRange(pos, data.attack.attackRange);
        foreach (var attackPos in data.attack.attackRange)
        {
            if (GameController.Instance.PlayerPos == pos + attackPos)
            {
                GameController.Instance.Damaged(dmg);
            }
        }
    }

    public override void Dead()
    {
        UnitController.Instance.DeadUnit(this);
        gameObject.SetActive(false); // 우선은 가리는 용도로만.
    }
}
