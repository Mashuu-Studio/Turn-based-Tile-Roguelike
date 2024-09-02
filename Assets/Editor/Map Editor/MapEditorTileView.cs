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

            // �ε�Ǵ� ��� ����.
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

        // ��Ÿ���� Ÿ���� �ޱ� ����.
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

        // Ÿ���� ������ �ٲٱ� ���� ��Ÿ�� ����.
        public void TileChanged(Tile.TileType type)
        {
            tile.SetType(type);
            ClearClassList();
            AddToClassList(GetTileType(tile.type));
        }
    }
}
