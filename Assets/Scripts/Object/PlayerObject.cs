using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : UnitObject
{
    [SerializeField] private Card[] attacks;

    private void Start()
    {
        for (int i = 0; i < 3; i++)
            Inventory.SetEquip(attacks[i], i);
    }

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
