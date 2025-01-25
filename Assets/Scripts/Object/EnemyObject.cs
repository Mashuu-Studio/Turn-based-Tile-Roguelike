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

        int rand = Random.Range(0, data.attacks.Count);
        var attack = data.attacks[rand];

        StageController.Instance.CurrentMap.ShowRange(pos, attack.attackRange);
        foreach (var attackPos in attack.attackRange)
        {
            if (PlayerController.Instance.PlayerPos == pos + attackPos)
            {
                PlayerController.Instance.Damaged(dmg);
            }
        }
    }

    public override void Dead()
    {
        UnitController.Instance.DeadUnit(this);
        gameObject.SetActive(false); // �켱�� ������ �뵵�θ�.
    }
}
