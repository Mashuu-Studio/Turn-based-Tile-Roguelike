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

        [MenuItem("Window/Custom Editor/Unit Editor")]
        public static void ShowExample()
        {
            UnitEditorWindow wnd = GetWindow<UnitEditorWindow>();
            wnd.titleContent = new GUIContent("UnitEditor");
        }

        public static void OpenWindow(Unit unit)
        {

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
        }

        public void PopulateView(Unit unit)
        {

        }

        private void OnSelectionChange()
        {
            
        }
    }
}
