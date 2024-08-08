using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AttackManager
{

    private static List<Attack> attacks;
    public static void Init()
    {
        attacks = new List<Attack>()
        {
            new Attack()
            {
                key = "+",
                attackRange = new List<Vector3Int>()
                {
                    Vector3Int.left,
                    Vector3Int.right,
                    Vector3Int.up,
                    Vector3Int.down
                }
            },

            new Attack()
            {
                key = "x",
                attackRange = new List<Vector3Int>()
                {
                    new Vector3Int(1, 1),
                    new Vector3Int(1, -1),
                    new Vector3Int(-1, 1),
                    new Vector3Int(-1, -1)
                }
            },

        };
    }

    public static Attack GetAttack(string key)
    {
        return attacks.Find(atk => atk.key == key);
    }
}
