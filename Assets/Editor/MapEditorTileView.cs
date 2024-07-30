using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using Unity.VisualScripting;

namespace MapEditor
{
    public class MapEditorTileView : Button
    {
        public Tile tile;
        Vector2Int pos;

        public MapEditorTileView(Tile tile, Vector2Int pos)
        {
            this.tile = tile;
            this.pos = pos;

            tile.OnTileChanged += TileChanged;
            TileChanged();
        }

        public string GetTileType()
        {
            switch (tile.type)
            {
                case Tile.TileType.NONE: return "Tile_None";
                case Tile.TileType.FLOOR: return "Tile_Floor";
                case Tile.TileType.OBSTARCLE: return "Tile_Obstacle";
            }
            return "";
        }

        public void TileChanged()
        {
            ClearClassList();
            AddToClassList(GetTileType());
        }
    }
}
