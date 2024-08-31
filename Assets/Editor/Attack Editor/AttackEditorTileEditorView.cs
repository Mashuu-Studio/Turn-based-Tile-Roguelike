using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;
using static Attack;

namespace AttackEditor
{
    public class AttackEditorTileEditorView : VisualElement
    {
        AttackEditorView attackEditorView;
        Attack attack;
        Vector3Int pos;
        AttackEditorSupportIcon[] supportIcons = new AttackEditorSupportIcon[Enum.GetValues(typeof(Attack.SpreadInfo.SpreadType)).Length];

        public new class UxmlFactory : UxmlFactory<AttackEditorTileEditorView, UxmlTraits> { }
        
        public void Select(Vector3Int pos)
        {
            this.pos = pos;

            if (childCount == 0) SetView();
            SetInfo();
        }

        private void SetView()
        {
            Attack.SpreadInfo.SpreadType[] types = Enum.GetValues(typeof(Attack.SpreadInfo.SpreadType)) as Attack.SpreadInfo.SpreadType[];

            for (int i =0;i < types.Length;i++) 
            {
                CreateIcon(types[i]);
            }
        }

        private void CreateIcon(Attack.SpreadInfo.SpreadType type)
        {
            AttackEditorSupportIcon icon = new AttackEditorSupportIcon(type);

            // 위치 조정.
            int size = 50;
            icon.style.width = icon.style.height = size;
            icon.style.position = Position.Absolute;
            Vector2Int iconPos = new Vector2Int(1, 1);
            switch (type)
            {
                case SpreadInfo.SpreadType.LEFT_UP: iconPos += Vector2Int.left + Vector2Int.down; break;
                case SpreadInfo.SpreadType.UP: iconPos += Vector2Int.down; break;
                case SpreadInfo.SpreadType.RIGHT_UP: iconPos += Vector2Int.right + Vector2Int.down; break;
                case SpreadInfo.SpreadType.RIGHT: iconPos += Vector2Int.right; break;
                case SpreadInfo.SpreadType.RIGHT_DOWN: iconPos += Vector2Int.right + Vector2Int.up; break;
                case SpreadInfo.SpreadType.DOWN: iconPos += Vector2Int.up; break;
                case SpreadInfo.SpreadType.LEFT_DOWN: iconPos += Vector2Int.left + Vector2Int.up; break;
                case SpreadInfo.SpreadType.LEFT: iconPos += Vector2Int.left; break;
            }

            icon.style.left = iconPos.x * 50;
            icon.style.top =  iconPos.y * 50;

            icon.RegisterCallback<PointerDownEvent>(evt =>
            {
                icon.isOn = !icon.isOn;
                Select(type, icon.isOn);
            });
            supportIcons[(int)type] = icon;
            Add(icon);
        }

        private void Select(Attack.SpreadInfo.SpreadType type, bool b)
        {
            // 해당 위치를 기준으로 정보 변경.
            attack.SetSpreadType(pos, type, b);
            attackEditorView.DrawView();
        }

        private void SetInfo()
        {
            // attack에서 pos 값을 넣어 정보 기입.
            
            for (int i = 0; i < supportIcons.Length; i++)
                supportIcons[i].isOn = false;

            if (attack.spreadInfos.ContainsKey(pos))
            {
                for (int i = 0; i < supportIcons.Length; i++)
                {
                    supportIcons[i].isOn = attack.spreadInfos[pos].spreadTypes[i];
                }
            }
        }

        private void ClearInfo()
        {
            // 해당 위치에 대한 정보를 다 날려줌.
        }

        internal void PopulateView(Attack attack, AttackEditorView attackEditorView)
        {
            this.attack = attack;
            this.attackEditorView = attackEditorView;
        }
    }
}