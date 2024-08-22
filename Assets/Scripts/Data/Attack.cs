using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "Attack Data", menuName = "Create Data/Attack")]
// 공격 타입 지정.
public class Attack : Data
{
    public List<Vector3Int> attackRange;
    public event Action OnChanged;

    public void AddRange(Vector3Int pos, bool b)
    {
        if (attackRange == null) attackRange = new List<Vector3Int>();

        if (b && !attackRange.Contains(pos)) attackRange.Add(pos);
        else if (!b && attackRange.Contains(pos)) attackRange.Remove(pos);

        AssetDatabase.SaveAssets();
    }

    public void Changed()
    {
        OnChanged?.Invoke();
    }
}