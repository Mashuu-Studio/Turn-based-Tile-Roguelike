using Codice.Client.BaseCommands;
using MapEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MapEditor
{
    public class TilePaletteView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<TilePaletteView, UxmlTraits> { }

        public Tile.TileType SelectedType { get { return selectedType; } }
        private Tile.TileType selectedType;
        private TileBrushView selectedBrush;
        public TilePaletteView()
        {
            SetPalette();
            ClearClassList();
            AddToClassList("Tile_Palette");
        }

        // View를 세팅해줌.
        public void SetPalette()
        {
            Clear();
            // 우선은 가진 타입의 종류로 세팅.
            Tile.TileType[] types = (Tile.TileType[])Enum.GetValues(typeof(Tile.TileType));

            for (int i = 0; i < types.Length; i++)
            {
                CreateBrushView(types[i]);
            }
        }

        // 타일 생성
        private void CreateBrushView(Tile.TileType type)
        {
            // 타일 사이즈는 기본적으로 50으로 설정.
            TileBrushView brushView = new TileBrushView(type);
            brushView.name = type.ToString();
            brushView.clicked += () => OnBrushSelected(brushView, type);
            if (selectedType == type) OnBrushSelected(brushView, type);

            Add(brushView);
        }

        private void OnBrushSelected(TileBrushView brush, Tile.TileType type)
        {
            selectedType = type;
            if (selectedBrush != null) selectedBrush.Select(false);
            selectedBrush = brush;
            selectedBrush.Select(true);
        }
    }
}