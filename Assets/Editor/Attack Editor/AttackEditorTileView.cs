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
        // 미선택, 선택, 방향에 의해 추가된 경우
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

        // 타일의 정보를 바꾸기 위해 스타일 변경.
        // 선택된 타일인지, 추가된 타일인지 체크.
        public void TileChanged(SelectState state)
        {
            // 만약에 추가일 때 이미 선택영역이면 할 필요가 업승ㅁ.
            if (!(state == SelectState.ADDED && this.state == SelectState.SELECTED)) this.state = state;
            // 색 온오프가 되어야함.
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
