using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AttackManager
{

    private static List<Attack> attacks;
    public static void Init()
    {
        attacks = Resources.LoadAll<Attack>("Attacks").ToList();
    }

    public static Attack GetAttack(string key)
    {
        return attacks.Find(atk => atk.key == key);
    }
}
