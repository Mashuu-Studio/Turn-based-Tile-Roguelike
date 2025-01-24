using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardEditor
{
    public class CardEditorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<CardEditorView, UxmlTraits> { }

        private Card card;
        internal void PopulateView(Card card)
        {
            this.card = card;
            card.OnTypeChanged += () => DrawView();

            DrawView();
        }

        public void DrawView()
        {
            Clear();
            // ī���� ��� ǥ��.

            // �����ư
            // �����ư�� ������ ��Ÿ�Կ� ���� card�� ������ �� Ÿ�Կ� �°� ������.
            var button = new Button();
            var label = new Label("SAVE");
            button.Add(label);
            button.RegisterCallback<ClickEvent>((evt) =>
            {
                AssetDatabase.SaveAssets();
            });
            Add(button);
        }
    }
}