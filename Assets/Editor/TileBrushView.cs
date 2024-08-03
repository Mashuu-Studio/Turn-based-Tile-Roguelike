using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MapEditor
{
    public class TileBrushView : Button
    {
        public Tile.TileType type;
        private static StyleColor unselectedColor = new StyleColor(Color.white);
        private static StyleColor selectedColor = new StyleColor(Color.gray);

        public TileBrushView(Tile.TileType type)
        {
            this.type = type;
            ClearClassList();
            AddToClassList(MapEditorTileView.GetTileType(type));
        }

        public void Select(bool selected)
        {
            style.unityBackgroundImageTintColor = selected ? selectedColor : unselectedColor;
        }
    }
}
