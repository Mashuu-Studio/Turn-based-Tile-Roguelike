using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData
{
    public string key;
}

public class Unit : ObjectData
{
    public enum Type { MELEE = 0, MELEERANGE, RANGE }
    public Type type;
    public int hp;
    public int dmg;
    public int range;
    public int speed;
    public int actions = 1;
}
