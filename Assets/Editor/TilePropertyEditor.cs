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
        typeProp = serializedObject.FindProperty("type");
    }

    public override void OnInspectorGUI()
    {
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
