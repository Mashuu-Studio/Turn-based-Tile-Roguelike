using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    private void Awake()
    {
        AttackManager.Init();
        UnitManager.Init();
    }
}
