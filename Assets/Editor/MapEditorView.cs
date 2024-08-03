using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MapEditor
{
    public class MapEditorView : VisualElement
    {
        private TilePaletteView tilePalette;
        public Action<MapEditorTileView> OnTileSelected;
        public Map map;
        private bool startBrushing;
        public new class UxmlFactory : UxmlFactory<MapEditorView, UxmlTraits> { }

        public MapEditorView()
        {
            ClearClassList();
            AddToClassList("MapEditorView");
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
            tileView.RegisterCallback<PointerDownEvent>(evt => tileView.TileChanged(tilePalette.SelectedType));
            tileView.RegisterCallback<PointerEnterEvent>(evt =>
            {
                if (startBrushing) tileView.TileChanged(tilePalette.SelectedType);
            });
            Add(tileView);
        }
    }
}