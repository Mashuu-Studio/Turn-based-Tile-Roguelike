using Codice.Client.BaseCommands;
using MapEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MapEditor
{
    public class TilePaletteView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<TilePaletteView, UxmlTraits> { }

        public static StyleColor UnselectedColor { get; private set; } = new StyleColor(Color.white);
        public static StyleColor SelectedColor { get; private set; } = new StyleColor(Color.gray);

        public Tile.TileType SelectedType { get { return selectedType; } }
        private Tile.TileType selectedType;
        private TileBrushView selectedBrush;

        public enum SupportItemType { HORIZONTAL_SYMMETRY = 0, VERTICAL_SYMMETRY }
        private SupportItemView[] supportItems;
        private const int PRESET_AMOUNT = 2; // 수동으로 세팅. 어차피 프리셋별로 타일을 채우는 방식 역시 직접 하드코딩해주어야 함.
        private SupportItemView[] presets;

        public bool GetSupportActivate(SupportItemType type)
        {
            return supportItems[(int)type].activate;
        }

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

            supportItems = new SupportItemView[Enum.GetValues(typeof(SupportItemType)).Length];
            for (int i = 0; i < supportItems.Length; i++)
            {
                supportItems[i] = CreateSupportItemView(GetStyleName((SupportItemType)i));
            }

            presets = new SupportItemView[PRESET_AMOUNT];
            for (int i = 0; i < PRESET_AMOUNT; i++)
            {
                presets[i] = CreateSupportItemView($"Map_Preset{i}", i);
            }
        }

        private string GetStyleName(SupportItemType type)
        {
            switch (type)
            {
                case SupportItemType.HORIZONTAL_SYMMETRY: return "Horizontal_Symmetry";
                case SupportItemType.VERTICAL_SYMMETRY: return "Vertical_Symmetry";
            }
            return "";
        }

        private SupportItemView CreateSupportItemView(string style, int presetNumber = -1)
        {
            SupportItemView supportItemView = new SupportItemView(style, presetNumber);
            supportItemView.name = style;
            supportItemView.clicked += () => supportItemView.Actiavte();

            Add(supportItemView);
            return supportItemView;
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