using MapEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

namespace AttackEditor
{
    public class AttackEditorView : VisualElement
    {
        private AttackEditorTileEditorView tileEditorView;
        public Action<AttackEditorTileView> OnTileSelected;
        public Attack attack;
        public AttackEditorTileView[,] tiles;

        public const int MAP_WIDTH = 11;
        public const int MAP_HEIGHT = 11;
        private Vector3Int center;
        public new class UxmlFactory : UxmlFactory<AttackEditorView, UxmlTraits> { }

        public AttackEditorView()
        {
            ClearClassList();
            AddToClassList("MapEditorView");
            center = new Vector3Int(MAP_WIDTH / 2, MAP_HEIGHT / 2);
            tiles = new AttackEditorTileView[MAP_WIDTH, MAP_HEIGHT];
        }

        // View �ʱ�ȭ
        internal void PopulateView(Attack attack, AttackEditorTileEditorView tileEditorView)
        {
            this.tileEditorView = tileEditorView;
            this.attack = attack;
            this.attack.OnChanged += DrawView;
            //this.map.LoadMap();
            // View�� ũ�Ⱑ �ٲ�� ����� ��ġ ���� �ٽ� ����.
            RegisterCallback<GeometryChangedEvent>(evt => SetView());

            SetView();
            DrawView();
        }

        // View�� ��������.
        public void SetView()
        {
            // ��ü ũ���� �߾ӿ� �R.
            Clear();
            Vector2 viewSize = new Vector2(layout.width, layout.height);
            Vector3Int pos = Vector3Int.zero;
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    // ������ �ּ�ȭ�� ���� �ݺ����� ������ ȣ���� �ƴ� ���� ����
                    pos.x = x;
                    pos.y = y;

                    // ������� ������ ������.
                    CreateTileView(pos, viewSize, pos == center);
                }
            }
        }

        // �� ���¸� �ǵ帮�� ���� ���·� ���� ����
        public void DrawView()
        {
            // ���� �̼��û��·�
            foreach (var tile in tiles)
            {
                TileChanged(tile.pos, AttackEditorTileView.SelectState.UNSELECTED, false);
            }

            // ������ ������ �κ��� �ٸ���.
            foreach (var pos in attack.attackRange)
            {
                // �������� ���� ���� �κ� �߰�.
                foreach (var p in attack.GetRange(pos + center, new Vector3Int(MAP_WIDTH, MAP_HEIGHT)))
                {
                    TileChanged(p, AttackEditorTileView.SelectState.ADDED, false);
                }
                TileChanged(pos + center, AttackEditorTileView.SelectState.SELECTED, false);
            }
        }

        // Ÿ�� ����
        private void CreateTileView(Vector3Int pos, Vector2 viewSize, bool unit)
        {
            // ���ϴ��� 0,0���� �����Ͽ� ������ MAP_WIDTH-1,MAP_HEIGHT-1�� ������.
            // Ÿ�� ������� �⺻������ 50���� ����.
            // ����� �̵����Ѿ� ��.
            AttackEditorTileView tileView = new AttackEditorTileView(pos, unit);
            float tileSize = 50;
            Vector2 startPos = new Vector2((viewSize.x - MAP_WIDTH * tileSize) / 2, (viewSize.y - MAP_HEIGHT * tileSize) / 2);

            tileView.style.position = Position.Absolute;
            tileView.style.left = startPos.x + pos.x * tileSize;
            tileView.style.top = startPos.y + (MAP_HEIGHT - pos.y) * tileSize; // �� ��ġ Ư���� ���ϴ� ����������

            // ������ ���� �ﰢ������ ����
            // �巡�� �Ǵ� ���� startBrusing�� üũ�� �� ����.
            tileView.RegisterCallback<PointerDownEvent>(evt =>
            {
                // ��Ŭ�� ��Ŭ���� ���� ������ ������ ����.
                AttackEditorTileView.SelectState state = AttackEditorTileView.SelectState.SELECTED;
                tileEditorView.Select(pos);
                if (evt.button == (int)MouseButton.RightMouse)
                {
                    // Attack�� �ִ� ������ ������.
                    attack.ClearSpreadInfo(pos);
                    state = AttackEditorTileView.SelectState.UNSELECTED;
                    tileEditorView.Clear();
                }
                TileChanged(pos, state);
                DrawView();
            });
            tiles[pos.x, pos.y] = tileView;
            Add(tileView);
        }

        private void TileChanged(Vector3Int pos, AttackEditorTileView.SelectState state, bool add = true)
        {
            tiles[pos.x, pos.y].TileChanged(state);
            if (add && state != AttackEditorTileView.SelectState.ADDED) attack.AddRange(pos - center, state == AttackEditorTileView.SelectState.SELECTED);
        }

        private void Save()
        {
            EditorUtility.SetDirty(attack);
            AssetDatabase.SaveAssets();
        }

        private void OnSelectionChange()
        {
            Save();
        }

        private void OnDisable()
        {
            Save();
        }
    }
}