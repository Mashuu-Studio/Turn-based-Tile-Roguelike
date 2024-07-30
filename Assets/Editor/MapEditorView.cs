using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MapEditor
{
    public class MapEditorView : VisualElement
    {
        public Action<MapEditorTileView> OnTileSelected;
        public Map map;
        public new class UxmlFactory : UxmlFactory<MapEditorView, UxmlTraits> { }

        public MapEditorView()
        {
            ClearClassList();
            AddToClassList("MapEditorView");
        }

        // View 초기화
        internal void PopulateView(Map map)
        {
            this.map = map;
            this.map.OnMapChanged += DrawView;
            this.map.LoadMap();


            DrawView();
        }

        // View를 세팅해줌.
        public void DrawView()
        {
            // 전체 크기의 중앙에 둚.
            Clear();
            Vector2 viewSize = new Vector2(style.width.value.value, style.height.value.value);
            Vector2Int mapSize = new Vector2Int(map.width, map.height);
            Vector2Int pos = Vector2Int.zero;
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
        private void CreateTileView(Tile tile, Vector2Int pos, Vector2 size, Vector2Int mapSize)
        {
            // 좌하단을 0,0으로 시작하여 우상단을 width-1,height-1로 마무리.
            // 타일 사이즈는 기본적으로 50으로 설정.
            // 가운데로 이동시켜야 함.
            MapEditorTileView tileView = new MapEditorTileView(tile, pos);
            tileView.style.position = Position.Absolute;
            tileView.style.left = pos.x * 55;
            tileView.style.top = pos.y * 55;
            tileView.clicked += () => OnTileSelected(tileView);
            Add(tileView);
        }
    }
}