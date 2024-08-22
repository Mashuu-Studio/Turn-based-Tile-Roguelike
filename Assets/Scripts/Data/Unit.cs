using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Unit Data", menuName = "Create Data/Unit")]
public class Unit : Data
{
    public int hp;
    public int dmg;
    public Attack attack;
}