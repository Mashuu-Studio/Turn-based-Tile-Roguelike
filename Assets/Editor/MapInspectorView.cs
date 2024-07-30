using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MapEditor
{
    public class MapInspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<MapInspectorView, UxmlTraits> { }

        Editor editor;
        public MapInspectorView()
        {
        }

        internal void PopulateView(Map map, MapEditorView editorView)
        {
            Clear();

            UnityEngine.Object.DestroyImmediate(editor);
            editor = Editor.CreateEditor(map);
            IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
            Add(container);

            map.OnMapChanged += editorView.DrawView;
        }
    }
}
