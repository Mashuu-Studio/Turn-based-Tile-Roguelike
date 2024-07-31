using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Map))]
public class MapPropertyMapEditor : Editor
{
    private SerializedProperty widthProp;
    private SerializedProperty heightProp;

    private void OnEnable()
    {
        if (target == null || serializedObject == null) return;
        widthProp = serializedObject.FindProperty("width");
        heightProp = serializedObject.FindProperty("height");
    }

    public override void OnInspectorGUI()
    {
        if (serializedObject == null) return;
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(widthProp);
        EditorGUILayout.PropertyField(heightProp);

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();

            Map map = (Map)target;
            map.SetMap();
        }
    }
}
