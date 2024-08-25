using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnitEditor
{
    public class UnitEditorWindow : EditorWindow
    {
        [OnOpenAsset()]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var target = EditorUtility.InstanceIDToObject(instanceID) as Unit;
            if (target)
            {
                OpenWindow(target);
                return true;
            }
            return false;
        }

        InspectorView inspectorView;
        UnitEditorAttackSelectorView attackSelectorView;
        UnitEditorAttackView attackView;

        [MenuItem("Window/Custom Editor/Unit Editor")]
        public static void ShowExample()
        {
            UnitEditorWindow wnd = GetWindow<UnitEditorWindow>();
            wnd.titleContent = new GUIContent("UnitEditor");
        }

        public static void OpenWindow(Unit unit)
        {
            UnitEditorWindow wnd = GetWindow<UnitEditorWindow>();
            wnd.titleContent = new GUIContent("UnitEditor");
            wnd.PopulateView(unit);
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Instantiate UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Unit Editor/UnitEditor.uxml");
            visualTree.CloneTree(root);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/Unit Editor/UnitEditor.uss");
            root.styleSheets.Add(styleSheet);

            inspectorView = root.Q<InspectorView>();
            attackSelectorView = root.Q<UnitEditorAttackSelectorView>();
            attackView = root.Q<UnitEditorAttackView>();
        }

        public void PopulateView(Unit unit)
        {
            if (unit)
            {
                inspectorView.PopulateView(unit);
                attackSelectorView.PopulateView(attackView);
                attackView.PopulateView(unit);
            }
        }

        private void OnSelectionChange()
        {
            Unit unit = Selection.activeObject as Unit;
            PopulateView(unit);
        }
    }
}
