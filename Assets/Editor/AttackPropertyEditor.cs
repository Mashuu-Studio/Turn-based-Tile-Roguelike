using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Attack))]
public class AttackPropertyEditor : Editor
{
    private int previousCount;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var atk = target as Attack;

        if (atk.attackRange != null && previousCount != atk.attackRange.Count)
        {
            atk.Changed();
            previousCount = atk.attackRange.Count;
        }
    }
}
