using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AttackEditor
{
    public class AttackEditorSupportIcon : VisualElement
    {
        public Attack.SpreadInfo.SpreadType type;
        public bool isOn
        {
            get { return m_isOn; }
            set
            {
                m_isOn = value;
                SetColor();
            }
        }
        private bool m_isOn;
        private static StyleColor UnselectedColor = new StyleColor(Color.white);
        private static StyleColor SelectedColor = new StyleColor(Color.gray);

        public AttackEditorSupportIcon(Attack.SpreadInfo.SpreadType type)
        {
            ClearClassList();
            AddToClassList(GetType(type));
        }

        private static string GetType(Attack.SpreadInfo.SpreadType type)
        {
            switch (type)
            {
                case Attack.SpreadInfo.SpreadType.LEFT_UP: return "LEFT_UP";
                case Attack.SpreadInfo.SpreadType.UP: return "UP";
                case Attack.SpreadInfo.SpreadType.RIGHT_UP: return "RIGHT_UP";
                case Attack.SpreadInfo.SpreadType.RIGHT: return "RIGHT";
                case Attack.SpreadInfo.SpreadType.RIGHT_DOWN: return "RIGHT_DOWN";
                case Attack.SpreadInfo.SpreadType.DOWN: return "DOWN";
                case Attack.SpreadInfo.SpreadType.LEFT_DOWN: return "LEFT_DOWN";
                case Attack.SpreadInfo.SpreadType.LEFT: return "LEFT";
            }
            return "";
        }

        public void SetColor()
        {
            style.unityBackgroundImageTintColor = isOn ? SelectedColor : UnselectedColor;
        }
    }
}
