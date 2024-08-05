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

        // View 초기화
        internal void PopulateView(Map map, TilePaletteView tilePalette)
        {
            this.tilePalette = tilePalette;

            this.map = map;
            this.map.OnMapChanged += DrawView;
            this.map.LoadMap();
            // View의 크기가 바뀌면 가운데로 위치 조정 다시 해줌.
            RegisterCallback<GeometryChangedEvent>(evt => DrawView());

            // Brush질을 진행할 수 있도록 해당 뷰를 선택해도 시작을 걸어줌.
            RegisterCallback<PointerDownEvent>(evt => startBrushing = true);
            RegisterCallback<PointerUpEvent>(evt => startBrushing = false);
            RegisterCallback<MouseLeaveEvent>(evt => startBrushing = false);

            DrawView();
        }

        // View를 세팅해줌.
        public void DrawView()
        {
            // 전체 크기의 중앙에 둚.
            Clear();
            Vector2 viewSize = new Vector2(layout.width, layout.height);
            Vector2Int mapSize = new Vector2Int(map.width, map.height);
            Vector2Int pos = Vector2Int.zero;
            tiles = new MapEditorTileView[map.width, map.height];
            for (int x = 0; x < map.width; x++)
            {
                for (int y = 0; y < map.height; y++)
                {
                    // 가비지 최소화를 위해 반복적인 생성자 호출이 아닌 변수 변경
                    pos.x = x;
                    pos.y = y;

                    CreateTileView(map.Tiles[x, y], pos, viewSize, mapSize);
                }
            }
        }

        // 타일 생성
        private void CreateTileView(Tile tile, Vector2Int pos, Vector2 viewSize, Vector2Int mapSize)
        {
            // 좌하단을 0,0으로 시작하여 우상단을 width-1,height-1로 마무리.
            // 타일 사이즈는 기본적으로 50으로 설정.
            // 가운데로 이동시켜야 함.
            MapEditorTileView tileView = new MapEditorTileView(tile, pos);
            float tileSize = 50;
            Vector2 startPos = new Vector2((viewSize.x - mapSize.x * tileSize) / 2, (viewSize.y - mapSize.y * tileSize) / 2);
            tileView.style.position = Position.Absolute;
            tileView.style.left = startPos.x + pos.x * tileSize;
            tileView.style.top = startPos.y + pos.y * tileSize;

            // 선택한 곳은 즉각적으로 변경
            // 드래그 되는 곳은 startBrusing을 체크한 뒤 변경.
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
            // 좌우 대칭 그리기
            if (horizontal)
            {
                tiles[x_inverse, pos.y].TileChanged(type);
            }

            // 상하 대칭 그리기
            if (vertical)
            {
                tiles[pos.x, y_inverse].TileChanged(type);
            }

            // 둘 다 되어있다면 정반대편도
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
                // 프리셋 1. 가장 바깥라인 빈공간으로 처리. 나머지는 바닥으로 처리.
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

                // 프리셋 2. 마름모꼴 형태로 바깥라인 빈공간으로 처리
                // 전체를 바닥으로 색칠.
                // 이 후 끝부분을 덮는 방식으로 진행.
                // 이 때, 좌상단 사분면을 칠하고 나머지를 대칭으로 색칠.
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