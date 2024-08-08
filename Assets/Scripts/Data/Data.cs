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

// ���� Ÿ�� ����.
public class Attack : Data
{
    public List<Vector3Int> attackRange;
}
