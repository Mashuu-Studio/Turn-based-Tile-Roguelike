using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : UnitObject
{
    // 임시 함수. Summon등과 통합되면 좋을 듯.
    public void SetPos(Vector3Int pos)
    {
        hp = 100;
        transform.localPosition = pos;
        this.pos = pos;
    }

    public override void Attack()
    {
        // 사용하는 능력에 따라 공격 방식 조정.
    }

    public override void Dead()
    {
        // 게임 오버
    }
}
