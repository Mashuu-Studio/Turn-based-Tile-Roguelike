using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitManager
{
    private static List<Unit> units;
    public static void Init()
    {
        units = new List<Unit>()
        {
            new Unit()
            {
                key = "UNIT.MELEE",
                type = Unit.Type.MELEE,
                hp = 4,
                dmg = 1,
                range = 1,
                speed = 5,
                actions = 2,
            },
            new Unit()
            {
                key = "UNIT.MELEERANGE",
                type = Unit.Type.MELEERANGE,
                hp = 5,
                dmg = 2,
                range = 1,
                speed = 3,
                actions = 1,
            },
            new Unit()
            {
                key = "UNIT.RANGE",
                type = Unit.Type.RANGE,
                hp = 2,
                dmg = 2,
                range = 3,
                speed = 4,
                actions = 1,
            },
        };
    }

    public static Unit GetUnit(string key)
    {
        return units.Find(unit => unit.key == key);
    }
}
