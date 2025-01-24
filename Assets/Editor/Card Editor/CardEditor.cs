using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardEditor
{
    public class CardEditorWindow : EditorWindow
    {
        [OnOpenAsset()]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var target = EditorUtility.InstanceIDToObject(instanceID) as Card;
            if (target)
            {
                OpenWindow(target);
                return true;
            }
            return false;
        }

        CardEditorView editorView;
        InspectorView inspectorView;

        [MenuItem("Window/Custom Editor/Card Editor")]
        public static void ShowExample()
        {
            CardEditorWindow window = GetWindow<CardEditorWindow>();
            window.titleContent = new GUIContent("CardEditor");
        }

        public static void OpenWindow(Card card)
        {
            CardEditorWindow window = GetWindow<CardEditorWindow>();
            window.titleContent = new GUIContent("CardEditor");
            window.PopulateView(card);
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Instantiate UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Card Editor/CardEditor.uxml");
            visualTree.CloneTree(root);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/Card Editor/CardEditor.uss");
            root.styleSheets.Add(styleSheet);

            editorView = root.Q<CardEditorView>();
            inspectorView = root.Q<InspectorView>();
        }

        public void PopulateView(Card card)
        {
            if (card)
            {
                editorView.PopulateView(card);
                inspectorView.PopulateView(card);
            }
        }

        private void OnSelectionChange()
        {
            Card card = Selection.activeObject as Card;
            PopulateView(card);
        }
    }
}
