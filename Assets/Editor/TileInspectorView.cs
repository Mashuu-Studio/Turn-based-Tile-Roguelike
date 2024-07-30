using MapEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MapEditor
{
    public class TileInspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<TileInspectorView, UxmlTraits> { }

        Editor editor;
        public TileInspectorView()
        {
        }

        internal void SelectTile(MapEditorTileView tileView)
        {
            Clear();

            UnityEngine.Object.DestroyImmediate(editor);
            editor = Editor.CreateEditor(tileView.tile);
            IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
            Add(container);
        }
    }
}