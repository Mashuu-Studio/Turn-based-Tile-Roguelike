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
                key = "TEST1",
                hp = 3,
                dmg = 1,
                attack = AttackManager.GetAttack("+")
            },
            new Unit()
            {
                key = "TEST2",
                hp = 3,
                dmg = 1,
                attack = AttackManager.GetAttack("x")
            },
        };
    }

    public static Unit GetUnit(string key)
    {
        return units.Find(unit => unit.key == key);
    }
}
