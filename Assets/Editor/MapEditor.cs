using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MapEditor
{
    public class MapEditor : EditorWindow
    {
        MapEditorView editorView;
        MapInspectorView mapInspectorView;
        TilePaletteView tilePaletteView;

        [MenuItem("Window/Editor/MapEditor")]
        public static void ShowExample()
        {
            MapEditor wnd = GetWindow<MapEditor>();
            wnd.titleContent = new GUIContent("MapEditor");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Instantiate UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/MapEditor.uxml");
            visualTree.CloneTree(root);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/MapEditor.uss");
            root.styleSheets.Add(styleSheet);

            editorView = root.Q<MapEditorView>();
            mapInspectorView = root.Q<MapInspectorView>();
            tilePaletteView = root.Q<TilePaletteView>();
        }

        private void OnSelectionChange()
        {
            Map map = Selection.activeObject as Map;
            if (map)
            {
                editorView.PopulateView(map, tilePaletteView);
                mapInspectorView.PopulateView(map, editorView);
            }
        }
    }
}
