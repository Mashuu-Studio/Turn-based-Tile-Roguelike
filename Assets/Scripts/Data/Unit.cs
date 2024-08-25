using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Data", menuName = "Create Data/Unit")]
public class Unit : Data
{
    public int hp;
    public int dmg;
    public List<Attack> attacks;

    public void AddAttack(Attack attack)
    {
        if (attacks == null) attacks = new List<Attack>();
        attacks.Add(attack);
    }

    public void RemoveAttack(Attack attack)
    {
        if (attack != null && attacks.Contains(attack))
        {
            attacks.Remove(attack);
        }
    }
}