using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : UnitObject
{
    // �ӽ� �Լ�. Summon��� ���յǸ� ���� ��.
    public void SetPos(Vector3Int pos)
    {
        hp = 100;
        transform.localPosition = pos;
        this.pos = pos;
    }

    public override void Attack()
    {
        // ����ϴ� �ɷ¿� ���� ���� ��� ����.
    }

    public override void Dead()
    {
        // ���� ����
    }
}
