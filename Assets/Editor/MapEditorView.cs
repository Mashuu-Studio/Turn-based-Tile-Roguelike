using Codice.CM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;

namespace MapEditor
{
    public class MapEditorView : VisualElement
    {
        private TilePaletteView tilePalette;
        public Action<MapEditorTileView> OnTileSelected;
        public Map map;
        public MapEditorTileView[,] tiles;
        private bool startBrushing;
        public new class UxmlFactory : UxmlFactory<MapEditorView, UxmlTraits> { }

        public MapEditorView()
        {
            ClearClassList();
            AddToClassList("MapEditorView");
            SupportItemView.OnUsePreset = Preset;
        }

        // View �ʱ�ȭ
        internal void PopulateView(Map map, TilePaletteView tilePalette)
        {
            this.tilePalette = tilePalette;

            this.map = map;
            this.map.OnMapChanged += DrawView;
            this.map.LoadMap();
            // View�� ũ�Ⱑ �ٲ�� ����� ��ġ ���� �ٽ� ����.
            RegisterCallback<GeometryChangedEvent>(evt => DrawView());

            // Brush���� ������ �� �ֵ��� �ش� �並 �����ص� ������ �ɾ���.
            RegisterCallback<PointerDownEvent>(evt => startBrushing = true);
            RegisterCallback<PointerUpEvent>(evt => startBrushing = false);
            RegisterCallback<MouseLeaveEvent>(evt => startBrushing = false);

            DrawView();
        }

        // View�� ��������.
        public void DrawView()
        {
            // ��ü ũ���� �߾ӿ� �R.
            Clear();
            Vector2 viewSize = new Vector2(layout.width, layout.height);
            Vector2Int mapSize = new Vector2Int(map.width, map.height);
            Vector2Int pos = Vector2Int.zero;
            tiles = new MapEditorTileView[map.width, map.height];
            for (int x = 0; x < map.width; x++)
            {
                for (int y = 0; y < map.height; y++)
                {
                    // ������ �ּ�ȭ�� ���� �ݺ����� ������ ȣ���� �ƴ� ���� ����
                    pos.x = x;
                    pos.y = y;

                    CreateTileView(map.Tiles[x, y], pos, viewSize, mapSize);
                }
            }
        }

        // Ÿ�� ����
        private void CreateTileView(Tile tile, Vector2Int pos, Vector2 viewSize, Vector2Int mapSize)
        {
            // ���ϴ��� 0,0���� �����Ͽ� ������ width-1,height-1�� ������.
            // Ÿ�� ������� �⺻������ 50���� ����.
            // ����� �̵����Ѿ� ��.
            MapEditorTileView tileView = new MapEditorTileView(tile, pos);
            float tileSize = 50;
            Vector2 startPos = new Vector2((viewSize.x - mapSize.x * tileSize) / 2, (viewSize.y - mapSize.y * tileSize) / 2);
            tileView.style.position = Position.Absolute;
            tileView.style.left = startPos.x + pos.x * tileSize;
            tileView.style.top = startPos.y + pos.y * tileSize;

            // ������ ���� �ﰢ������ ����
            // �巡�� �Ǵ� ���� startBrusing�� üũ�� �� ����.
            tileView.RegisterCallback<PointerDownEvent>(evt => TileChanged(pos, tilePalette.SelectedType));
            tileView.RegisterCallback<PointerEnterEvent>(evt =>
            {
                if (startBrushing) TileChanged(pos, tilePalette.SelectedType);
            });
            tiles[pos.x, pos.y] = tileView;
            Add(tileView);
        }

        private void TileChanged(Vector2Int pos, Tile.TileType type)
        {
            tiles[pos.x, pos.y].TileChanged(type);

            bool horizontal = tilePalette.GetSupportActivate(TilePaletteView.SupportItemType.HORIZONTAL_SYMMETRY);
            bool vertical = tilePalette.GetSupportActivate(TilePaletteView.SupportItemType.VERTICAL_SYMMETRY);

            int x_inverse = map.width - 1 - pos.x;
            int y_inverse = map.height - 1 - pos.y;
            // �¿� ��Ī �׸���
            if (horizontal)
            {
                tiles[x_inverse, pos.y].TileChanged(type);
            }

            // ���� ��Ī �׸���
            if (vertical)
            {
                tiles[pos.x, y_inverse].TileChanged(type);
            }

            // �� �� �Ǿ��ִٸ� ���ݴ���
            if (horizontal && vertical)
            {
                tiles[x_inverse, y_inverse].TileChanged(type);
            }

        }

        public void Preset(int index)
        {
            Vector2Int pos = Vector2Int.zero;
            switch (index)
            {
                // ������ 1. ���� �ٱ����� ��������� ó��. �������� �ٴ����� ó��.
                case 0:
                    for (int x = 0; x < map.width; x++)
                    {
                        for (int y = 0; y < map.height; y++)
                        {
                            pos.x = x;
                            pos.y = y;
                            if (x == 0 || y == 0
                                || x == map.width - 1 || y == map.height - 1)
                            {
                                TileChanged(pos, Tile.TileType.NONE);
                            }
                            else
                            {
                                TileChanged(pos, Tile.TileType.FLOOR);
                            }
                        }
                    }
                    break;

                // ������ 2. ������� ���·� �ٱ����� ��������� ó��
                // ��ü�� �ٴ����� ��ĥ.
                // �� �� ���κ��� ���� ������� ����.
                // �� ��, �»�� ��и��� ĥ�ϰ� �������� ��Ī���� ��ĥ.
                case 1:
                    for (int x = 0; x < map.width; x++)
                    {
                        for (int y = 0; y < map.height; y++)
                        {
                            pos.x = x;
                            pos.y = y;
                            TileChanged(pos, Tile.TileType.FLOOR);
                        }
                    }
                    
                    for (int x = 0; x < (map.width - 1) / 2; x++)
                    {
                        for (int y = 0; y < (map.height - 1) / 2; y++)
                        {
                            if (y >= ((map.height - 1) / 2) - x
                                || x >= (map.width - 1) / 2 - y) break;
                            int x_inverse = map.width - 1 - x;
                            int y_inverse = map.height - 1 - y;

                            pos.x = x;
                            pos.y = y;
                            TileChanged(pos, Tile.TileType.NONE);
                            pos.x = x_inverse;
                            TileChanged(pos, Tile.TileType.NONE);
                            pos.y = y_inverse;
                            TileChanged(pos, Tile.TileType.NONE);
                            pos.x = x;
                            TileChanged(pos, Tile.TileType.NONE);
                        }
                    }
                    break;
            }
        }
    }
}