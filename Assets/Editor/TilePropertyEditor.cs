using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Tile))]
public class TilePropertyMapEditor : Editor
{
    private SerializedProperty typeProp;

    private void OnEnable()
    {
        if (target == null || serializedObject == null) return;
        typeProp = serializedObject.FindProperty("type");
    }

    public override void OnInspectorGUI()
    {
        if (serializedObject == null) return;
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(typeProp);

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();

            Tile tile = (Tile)target;
            tile.TileChanged();
        }
    }
}
