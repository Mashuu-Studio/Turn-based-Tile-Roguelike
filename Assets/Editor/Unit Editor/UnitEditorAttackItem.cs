using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnitEditor
{
    public class UnitEditorAttackItem : VisualElement
    {
        Attack attack;
        bool selected;

        private static StyleColor UnselectedColor = new StyleColor(Color.white);
        private static StyleColor SelectedColor = new StyleColor(Color.gray);

        public UnitEditorAttackItem(Attack attack)
        {
            this.attack = attack;
            selected = false;
            ClearClassList();
            AddToClassList("Attack_Selector_Item");

            var label = new Label(attack.key);
            label.style.fontSize = 18;
            label.style.color = Color.black;
            label.style.alignContent = Align.Center;
            Add(label);
        }

        public void Select(UnitEditorAttackView attackView, bool b)
        {
            selected = b;
            if (selected) attackView.DrawView(attack);
            style.unityBackgroundImageTintColor = selected ? SelectedColor : UnselectedColor;
        }
    }
}