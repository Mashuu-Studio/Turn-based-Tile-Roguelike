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

        public MapEditorTileView(Tile tile, Map map, Vector2Int pos)
        {
            this.tile = tile;
            this.pos = pos;
            TileChanged(tile.type);

            // 로드되는 경우 세팅.
            if (tile.type == Tile.TileType.UNIT)
            {
                var label = new Label();
                label.style.fontSize = 10;
                label.style.color = Color.black;
                var enemy = map.GetEnemy((Vector3Int)pos);
                if (enemy != null) label.text = enemy.key;
                Add(label);
            }
        }

        // 스타일의 타입을 받기 위함.
        public static string GetTileType(Tile.TileType type)
        {
            switch (type)
            {
                case Tile.TileType.NONE: return "Tile_None";
                case Tile.TileType.FLOOR: return "Tile_Floor";
                case Tile.TileType.OBSTARCLE: return "Tile_Obstacle";
                case Tile.TileType.UNIT: return "Tile_Add_Unit";
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
