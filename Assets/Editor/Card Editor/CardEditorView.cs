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
            // 카드의 모습 표기.

            // 저장버튼
            // 저장버튼을 누르면 현타입에 따라서 card의 정보를 각 타입에 맞게 저장함.
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