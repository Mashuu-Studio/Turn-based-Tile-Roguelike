using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public string key;
}

public class Unit : Data
{
    public int hp;
    public int dmg;
    public Attack attack;
}

// 공격 타입 지정.
public class Attack : Data
{
    public List<Vector3Int> attackRange;
}
