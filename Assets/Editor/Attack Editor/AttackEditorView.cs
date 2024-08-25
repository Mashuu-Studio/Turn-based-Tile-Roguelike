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
        public Action<AttackEditorTileView> OnTileSelected;
        public Attack attack;
        public AttackEditorTileView[,] tiles;
        private bool startBrushing;
        private bool brushSelected;

        public const int MAP_WIDTH = 11;
        public const int MAP_HEIGHT = 11;
        private Vector3Int center;
        public new class UxmlFactory : UxmlFactory<AttackEditorView, UxmlTraits> { }

        public AttackEditorView()
        {
            ClearClassList();
            AddToClassList("MapEditorView");
            center = new Vector3Int(MAP_WIDTH/ 2, MAP_HEIGHT / 2);
            tiles = new AttackEditorTileView[MAP_WIDTH, MAP_HEIGHT];
        }

        // View �ʱ�ȭ
        internal void PopulateView(Attack attack)
        {
            this.attack = attack;
            this.attack.OnChanged += DrawView;
            //this.map.LoadMap();
            // View�� ũ�Ⱑ �ٲ�� ����� ��ġ ���� �ٽ� ����.
            RegisterCallback<GeometryChangedEvent>(evt => SetView());

            // Brush���� ������ �� �ֵ��� �ش� �並 �����ص� ������ �ɾ���.
            RegisterCallback<PointerDownEvent>(evt => startBrushing = true);
            RegisterCallback<PointerUpEvent>(evt => startBrushing = false);
            RegisterCallback<MouseLeaveEvent>(evt => startBrushing = false);

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
                    CreateTileView(pos, viewSize, pos == center, attack.attackRange.Contains(pos - center));
                }
            }
        }

        // �� ���¸� �ǵ帮�� ���� ���·� ���� ����
        public void DrawView()
        {
            bool prev = brushSelected;
            // ���� �̼��û��·�
            brushSelected = false;
            foreach (var tile in tiles)
            {
                TileChanged(tile.pos, false);
            }

            // range�� �ִ� �κи� �� �߰�
            brushSelected = true;
            foreach (var pos in attack.attackRange)
            {
                TileChanged(pos + center, false);
            }

            brushSelected = prev;
        }

        // Ÿ�� ����
        private void CreateTileView(Vector3Int pos, Vector2 viewSize, bool unit, bool selected)
        {
            // ���ϴ��� 0,0���� �����Ͽ� ������ MAP_WIDTH-1,MAP_HEIGHT-1�� ������.
            // Ÿ�� ������� �⺻������ 50���� ����.
            // ����� �̵����Ѿ� ��.
            AttackEditorTileView tileView = new AttackEditorTileView(pos, unit, selected);
            float tileSize = 50;
            Vector2 startPos = new Vector2((viewSize.x - MAP_WIDTH * tileSize) / 2, (viewSize.y - MAP_HEIGHT * tileSize) / 2);
           
            tileView.style.position = Position.Absolute;
            tileView.style.left = startPos.x + pos.x * tileSize;
            tileView.style.top = startPos.y + pos.y * tileSize;

            // ������ ���� �ﰢ������ ����
            // �巡�� �Ǵ� ���� startBrusing�� üũ�� �� ����.
            tileView.RegisterCallback<PointerDownEvent>(evt =>
            {
                // ��Ŭ�� ��Ŭ���� ���� ������ ������ ����.
                brushSelected = evt.button == (int)MouseButton.LeftMouse;
                TileChanged(pos);
            });
            tileView.RegisterCallback<PointerMoveEvent>(evt =>
            {
                if (startBrushing)
                {
                    // �߰��� Ŭ���� ����Ǹ� �ν�. On Off�� �ٲ�.
                    if (brushSelected && Event.current.type == EventType.MouseDown && Event.current.button == 1) brushSelected = false;
                    else if (!brushSelected && Event.current.type == EventType.MouseDown && Event.current.button == 0) brushSelected = true;
                    
                    // ���࿡ ��Ŭ������ ����Ǵ� �� ���� ��ư�� Ǯ���� �Ǹ� �� ���� �ν�. On Off ����.
                    if (brushSelected && Event.current.type == EventType.MouseUp && Event.current.button == 0) brushSelected = false;
                    else if (!brushSelected && Event.current.type == EventType.MouseUp && Event.current.button == 1) brushSelected = true;
                    TileChanged(pos);
                }
            });
            tiles[pos.x, pos.y] = tileView;
            Add(tileView);
        }

        private void TileChanged(Vector3Int pos, bool add = true)
        {
            tiles[pos.x, pos.y].TileChanged(brushSelected);
            // ����ο��� ��쿡�� �߰����� �ʵ���.
            if (add) attack.AddRange(pos - center, tiles[pos.x, pos.y].selected);
            /*
            bool horizontal = tilePalette.GetSupportActivate(TilePaletteView.SupportItemType.HORIZONTAL_SYMMETRY);
            bool vertical = tilePalette.GetSupportActivate(TilePaletteView.SupportItemType.VERTICAL_SYMMETRY);

            int x_inverse = map.MAP_WIDTH - 1 - pos.x;
            int y_inverse = map.MAP_HEIGHT - 1 - pos.y;
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
            }*/
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