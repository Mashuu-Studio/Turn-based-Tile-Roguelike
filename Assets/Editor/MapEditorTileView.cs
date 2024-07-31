using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MapEditor
{
    public class MapEditorTileView : VisualElement
    {
        public Tile tile;
        Vector2Int pos;

        public MapEditorTileView(Tile tile, Vector2Int pos)
        {
            this.tile = tile;
            this.pos = pos;
            TileChanged(tile.type);
        }

        // 스타일의 타입을 받기 위함.
        public static string GetTileType(Tile.TileType type)
        {
            switch (type)
            {
                case Tile.TileType.NONE: return "Tile_None";
                case Tile.TileType.FLOOR: return "Tile_Floor";
                case Tile.TileType.OBSTARCLE: return "Tile_Obstacle";
            }
            return "";
        }

        // 타일의 정보를 바꾸기 위해 스타일 변경.
        public void TileChanged(Tile.TileType type)
        {
            tile.SetType(type);
            ClearClassList();
            AddToClassList(GetTileType(tile.type));
        }
    }
}
