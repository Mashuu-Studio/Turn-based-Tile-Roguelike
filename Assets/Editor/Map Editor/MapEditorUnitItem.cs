using System.Collections;
using System.Collections.Generic;
using UnitEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MapEdtior
{
    public class MapEditorUnitItem : VisualElement
    {
        Unit unit;
        bool selected;

        private static StyleColor UnselectedColor = new StyleColor(Color.white);
        private static StyleColor SelectedColor = new StyleColor(Color.gray);

        public MapEditorUnitItem(Unit unit)
        {
            this.unit = unit;
            selected = false;
            ClearClassList();
            AddToClassList("Unit_Selector_Item");

            var label = new Label(unit.key);
            label.style.fontSize = 18;
            label.style.color = Color.black;
            label.style.alignContent = Align.Center;
            Add(label);
        }

        public void Select(UnitEditorAttackView attackView, bool b)
        {
            selected = b;
            // Ãß°¡
            // Ãß°¡µÈ À¯´Ö ¼ö ºä
            style.unityBackgroundImageTintColor = selected ? SelectedColor : UnselectedColor;
        }
    }
}

