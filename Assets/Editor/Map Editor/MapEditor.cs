using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.UIElements;

namespace MapEditor
{
    public class MapEditorWindow : EditorWindow
    {
        [OnOpenAsset()]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var target = EditorUtility.InstanceIDToObject(instanceID) as Map;
            if (target)
            {
                OpenWindow(target);
                return true;
            }
            return false;
        }

        MapEditorView editorView;
        InspectorView mapInspectorView;
        TilePaletteView tilePaletteView;
        MapEditorUnitSelectorView unitSelectorView;

        [MenuItem("Window/Custom Editor/Map Editor")]
        public static void ShowExample()
        {
            var window = GetWindow<MapEditorWindow>();
            window.titleContent = new GUIContent("MapEditor");
        }

        public static void OpenWindow(Map map)
        {
            var window = GetWindow<MapEditorWindow>();
            window.titleContent = new GUIContent("MapEditor");
            window.PopulateView(map);
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Instantiate UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Map Editor/MapEditor.uxml");
            visualTree.CloneTree(root);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/Map Editor/MapEditor.uss");
            root.styleSheets.Add(styleSheet);

            editorView = root.Q<MapEditorView>();
            mapInspectorView = root.Q<InspectorView>();
            tilePaletteView = root.Q<TilePaletteView>();
            unitSelectorView = root.Q<MapEditorUnitSelectorView>();
        }

        public void PopulateView(Map map)
        {
            if (map)
            {
                editorView.PopulateView(map, tilePaletteView, unitSelectorView);
                mapInspectorView.PopulateView(map);
                unitSelectorView.PopulateView(editorView);
            }
        }

        private void OnSelectionChange()
        {
            Map map = Selection.activeObject as Map;
            PopulateView(map);
        }
    }
}
