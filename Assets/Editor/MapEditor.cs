using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MapEditor
{
    public class MapEditor : EditorWindow
    {
        MapEditorView editorView;
        MapInspectorView mapInspectorView;
        TileInspectorView tileInspectorView;

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
            tileInspectorView = root.Q<TileInspectorView>();
            editorView.OnTileSelected = tileInspectorView.SelectTile;
        }

        private void OnSelectionChange()
        {
            Map map = Selection.activeObject as Map;
            if (map)
            {
                editorView.PopulateView(map);
                mapInspectorView.PopulateView(map, editorView);
            }
        }
    }
}
