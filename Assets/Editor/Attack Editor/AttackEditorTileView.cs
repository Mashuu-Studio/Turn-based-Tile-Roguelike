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

        // 타일의 정보를 바꾸기 위해 스타일 변경.
        public void TileChanged(bool b)
        {
            selected = b;
            // 색 온오프가 되어야함.
            style.unityBackgroundImageTintColor = selected ? SelectedColor : UnselectedColor;
        }
    }
}
