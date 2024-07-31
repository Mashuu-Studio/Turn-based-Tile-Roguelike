using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MapEditor
{
    public class TileBrushView : Button
    {
        public Tile.TileType type;

        public TileBrushView(Tile.TileType type)
        {
            this.type = type;
            ClearClassList();
            AddToClassList(MapEditorTileView.GetTileType(type));
        }
    }
}
