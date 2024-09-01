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

        IntegerField rangeField;
        IntegerField intervalField;
        public new class UxmlFactory : UxmlFactory<AttackEditorTileEditorView, UxmlTraits> { }

        public void Select(Vector3Int pos)
        {
            this.pos = pos;

            if (childCount == 0) SetView();
            SetInfo();
        }

        private void SetView()
        {
            // range, interval ���� ��ǲ �ʵ�
            rangeField = new IntegerField();
            rangeField.maxLength = 1;
            rangeField.RegisterCallback<ChangeEvent<int>>(evt =>
            {
                attack.SetRange(pos, evt.newValue);
                attackEditorView.DrawView();
            });

            rangeField.label = "RANGE";
            Add(rangeField);

            intervalField = new IntegerField();
            intervalField.maxLength = 1;
            intervalField.RegisterCallback<ChangeEvent<int>>(evt =>
            {
                attack.SetInterval(pos, evt.newValue);
                attackEditorView.DrawView();
            });

            intervalField.label = "INTERVAL";
            Add(intervalField);


            Attack.SpreadInfo.SpreadType[] types = Enum.GetValues(typeof(Attack.SpreadInfo.SpreadType)) as Attack.SpreadInfo.SpreadType[];

            for (int i = 0; i < types.Length; i++)
            {
                CreateIcon(types[i]);
            }

            // Clear ��ư
            {
                Button clearButton = new Button();
                var label = new Label("CLEAR");
                label.style.fontSize = 18;
                label.style.color = Color.black;
                label.style.alignContent = Align.Center;
                clearButton.Add(label);

                clearButton.style.position = Position.Absolute;
                clearButton.style.top = 250;
                clearButton.RegisterCallback<ClickEvent>(evt =>
                {
                    ClearInfo();
                    SetInfo();
                    attackEditorView.DrawView();
                });

                Add(clearButton);

            }

        }

        private void CreateIcon(Attack.SpreadInfo.SpreadType type)
        {
            AttackEditorSupportIcon icon = new AttackEditorSupportIcon(type);

            // ��ġ ����.
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

            icon.style.left = iconPos.x * 50 + 50;
            icon.style.top = iconPos.y * 50 + 75; // ���� �ִ� Ÿ�� ��ġ �����ؼ�.

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
            // �ش� ��ġ�� �������� ���� ����.
            attack.SetSpreadType(pos, type, b);
            attackEditorView.DrawView();
        }

        private void SetInfo()
        {
            // attack���� pos ���� �־� ���� ����.
            if (attack.spreadInfos.ContainsKey(pos))
            {
                var info = attack.spreadInfos[pos];
                rangeField.value = info.range;
                intervalField.value = info.interval;
                for (int i = 0; i < supportIcons.Length; i++)
                {
                    supportIcons[i].isOn = info.spreadTypes[i];
                }
            }
            else
            {
                rangeField.value = 0;
                intervalField.value = 0;

                for (int i = 0; i < supportIcons.Length; i++)
                    supportIcons[i].isOn = false;
            }
        }

        private void ClearInfo()
        {
            // �ش� ��ġ�� ���� ������ �� ������.
            attack.ClearSpreadInfo(pos);
        }

        internal void PopulateView(Attack attack, AttackEditorView attackEditorView)
        {
            this.attack = attack;
            this.attackEditorView = attackEditorView;
        }
    }
}