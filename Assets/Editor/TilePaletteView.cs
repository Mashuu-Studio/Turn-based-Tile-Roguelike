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
        public TilePaletteView()
        {
            SetPalette();
            ClearClassList();
            AddToClassList("Tile_Palette");
        }

        // View�� ��������.
        public void SetPalette()
        {
            Clear();
            // �켱�� ���� Ÿ���� ������ ����.
            Tile.TileType[] types = (Tile.TileType[])Enum.GetValues(typeof(Tile.TileType));

            for (int i = 0; i < types.Length; i++)
            {
                CreateBrushView(types[i]);
            }
        }

        // Ÿ�� ����
        private void CreateBrushView(Tile.TileType type)
        {
            // Ÿ�� ������� �⺻������ 50���� ����.
            TileBrushView brushView = new TileBrushView(type);
            brushView.clicked += () => OnBrushSelected(type);

            Add(brushView);
        }

        private void OnBrushSelected(Tile.TileType type)
        {
            selectedType = type;
        }
    }
}