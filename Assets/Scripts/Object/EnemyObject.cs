using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : UnitObject
{
    public override void Attack()
    {
        // Ư�� ��ġ�� �����ؾ���.
        // ������ �ش� ������ ��� �ѱ��� ���.
        // ���� Ȱ��ȭ �� ���� �� �� �ִٸ� ���� ��.

        // �켱 �ӽ÷� ���� Player�� ������ �ִ� GameController�� ���������� ����
        // �Ŀ��� ���������� ���� ����.

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
        gameObject.SetActive(false); // �켱�� ������ �뵵�θ�.
    }
}
