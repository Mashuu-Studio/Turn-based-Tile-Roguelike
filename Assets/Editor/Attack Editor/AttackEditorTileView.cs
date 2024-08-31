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
        // �̼���, ����, ���⿡ ���� �߰��� ���
        public enum SelectState { UNSELECTED = 0, SELECTED, ADDED }
        private static StyleColor UnselectedColor = new StyleColor(Color.white);
        private static StyleColor SelectedColor = new StyleColor(Color.green);
        private static StyleColor AddedColor = new StyleColor(Color.red);

        public SelectState state;
        public Vector3Int pos;

        public AttackEditorTileView(Vector3Int pos, bool unit)
        {
            state = SelectState.UNSELECTED;
            this.pos = pos;
            ClearClassList();
            AddToClassList(unit ? "Tile_Unit" : "Tile");
            TileChanged(state);
        }

        // Ÿ���� ������ �ٲٱ� ���� ��Ÿ�� ����.
        // ���õ� Ÿ������, �߰��� Ÿ������ üũ.
        public void TileChanged(SelectState state)
        {
            // ���࿡ �߰��� �� �̹� ���ÿ����̸� �� �ʿ䰡 ���¤�.
            if (!(state == SelectState.ADDED && this.state == SelectState.SELECTED)) this.state = state;
            // �� �¿����� �Ǿ����.
            StyleColor color = UnselectedColor;
            switch (this.state)
            {
                case SelectState.SELECTED: color = SelectedColor; break;
                case SelectState.ADDED: color = AddedColor; break;
            }
            style.unityBackgroundImageTintColor = color;
        }
    }
}
