using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.UIElements;

namespace MapEditor
{
    public class MapEditorWindow : EditorWindow
    {
        MapEditorView editorView;
        MapInspectorView mapInspectorView;
        TilePaletteView tilePaletteView;

        [MenuItem("Window/Custom Editor/MapEditor")]
        public static void ShowExample()
        {
            MapEditorWindow wnd = GetWindow<MapEditorWindow>();
            wnd.titleContent = new GUIContent("MapEditor");
        }

        public static void OpenWindow(Map map)
        {
            var window = GetWindow<MapEditorWindow>();
            window.PopulateView(map);
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

        public void PopulateView(Map map)
        {
            if (map)
            {
                editorView.PopulateView(map, tilePaletteView);
                mapInspectorView.PopulateView(map, editorView);
            }
        }

        private void OnSelectionChange()
        {
            Map map = Selection.activeObject as Map;
            PopulateView(map);
        }
    }
}
