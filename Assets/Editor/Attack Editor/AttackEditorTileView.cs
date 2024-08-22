using MapEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.UIElements;

namespace AttackEditor
{
    public class AttackEditorTileView : VisualElement
    {
        private static StyleColor UnselectedColor = new StyleColor(Color.white);
        private static StyleColor SelectedColor = new StyleColor(Color.red);

        public bool selected;
        public Vector3Int pos;

        public AttackEditorTileView(Vector3Int pos, bool unit, bool selected)
        {
            this.selected = selected;
            this.pos = pos;
            ClearClassList();
            AddToClassList(unit ? "Tile_Unit" : "Tile");
            TileChanged(selected);
        }

        // Ÿ���� ������ �ٲٱ� ���� ��Ÿ�� ����.
        public void TileChanged(bool b)
        {
            selected = b;
            // �� �¿����� �Ǿ����.
            style.unityBackgroundImageTintColor = selected ? SelectedColor : UnselectedColor;
        }
    }
}
