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

        // ��Ÿ���� Ÿ���� �ޱ� ����.
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

        // Ÿ���� ������ �ٲٱ� ���� ��Ÿ�� ����.
        public void TileChanged(Tile.TileType type)
        {
            tile.SetType(type);
            ClearClassList();
            AddToClassList(GetTileType(tile.type));
        }
    }
}
