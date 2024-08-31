using Codice.Client.BaseCommands;
using UnitEditor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace AttackEditor
{
    public class AttackEditorWindow : EditorWindow
    {
        [OnOpenAsset()]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var target = EditorUtility.InstanceIDToObject(instanceID) as Attack;
            if (target)
            {
                OpenWindow(target);
                return true;
            }
            return false;
        }

        AttackEditorView editorView;
        AttackEditorTileEditorView tileEditorView;
        InspectorView inspectorView;

        [MenuItem("Window/Custom Editor/Attack Editor")]
        public static void ShowExample()
        {
            AttackEditorWindow window = GetWindow<AttackEditorWindow>();
            window.titleContent = new GUIContent("AttackEditor");
        }

        public static void OpenWindow(Attack attack)
        {
            AttackEditorWindow window = GetWindow<AttackEditorWindow>();
            window.titleContent = new GUIContent("AttackEditor");
            window.PopulateView(attack);
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Instantiate UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Attack Editor/AttackEditor.uxml");
            visualTree.CloneTree(root);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/Attack Editor/AttackEditor.uss");
            root.styleSheets.Add(styleSheet);

            editorView = root.Q<AttackEditorView>();
            tileEditorView = root.Q<AttackEditorTileEditorView>();
            inspectorView = root.Q<InspectorView>();
        }

        public void PopulateView(Attack attack)
        {
            if (attack)
            {
                editorView.PopulateView(attack, tileEditorView);
                tileEditorView.PopulateView(attack, editorView);
                inspectorView.PopulateView(attack);
            }
        }

        private void OnSelectionChange()
        {
            Attack attack = Selection.activeObject as Attack;
            PopulateView(attack);
        }
    }
}
