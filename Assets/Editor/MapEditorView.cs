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

        // View �ʱ�ȭ
        internal void PopulateView(Map map)
        {
            this.map = map;
            this.map.OnMapChanged += DrawView;
            this.map.LoadMap();


            DrawView();
        }

        // View�� ��������.
        public void DrawView()
        {
            // ��ü ũ���� �߾ӿ� �R.
            Clear();
            Vector2 viewSize = new Vector2(style.width.value.value, style.height.value.value);
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
        private void CreateTileView(Tile tile, Vector2Int pos, Vector2 size, Vector2Int mapSize)
        {
            // ���ϴ��� 0,0���� �����Ͽ� ������ width-1,height-1�� ������.
            // Ÿ�� ������� �⺻������ 50���� ����.
            // ����� �̵����Ѿ� ��.
            MapEditorTileView tileView = new MapEditorTileView(tile, pos);
            tileView.style.position = Position.Absolute;
            tileView.style.left = pos.x * 55;
            tileView.style.top = pos.y * 55;
            tileView.clicked += () => OnTileSelected(tileView);
            Add(tileView);
        }
    }
}