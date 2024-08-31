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

        // View 초기화
        internal void PopulateView(Attack attack, AttackEditorTileEditorView tileEditorView)
        {
            this.tileEditorView = tileEditorView;
            this.attack = attack;
            this.attack.OnChanged += DrawView;
            //this.map.LoadMap();
            // View의 크기가 바뀌면 가운데로 위치 조정 다시 해줌.
            RegisterCallback<GeometryChangedEvent>(evt => SetView());

            SetView();
            DrawView();
        }

        // View를 세팅해줌.
        public void SetView()
        {
            // 전체 크기의 중앙에 둚.
            Clear();
            Vector2 viewSize = new Vector2(layout.width, layout.height);
            Vector3Int pos = Vector3Int.zero;
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    // 가비지 최소화를 위해 반복적인 생성자 호출이 아닌 변수 변경
                    pos.x = x;
                    pos.y = y;

                    // 가운데에는 유닛이 들어가야함.
                    CreateTileView(pos, viewSize, pos == center);
                }
            }
        }

        // 현 상태를 건드리지 않은 상태로 색만 변경
        public void DrawView()
        {
            // 전부 미선택상태로
            foreach (var tile in tiles)
            {
                TileChanged(tile.pos, AttackEditorTileView.SelectState.UNSELECTED, false);
            }

            // 실제로 선택한 부분은 다른색.
            foreach (var pos in attack.attackRange)
            {
                // 방향으로 인해 생긴 부분 추가.
                foreach (var p in attack.GetRange(pos + center, new Vector3Int(MAP_WIDTH, MAP_HEIGHT)))
                {
                    TileChanged(p, AttackEditorTileView.SelectState.ADDED, false);
                }
                TileChanged(pos + center, AttackEditorTileView.SelectState.SELECTED, false);
            }
        }

        // 타일 생성
        private void CreateTileView(Vector3Int pos, Vector2 viewSize, bool unit)
        {
            // 좌하단을 0,0으로 시작하여 우상단을 MAP_WIDTH-1,MAP_HEIGHT-1로 마무리.
            // 타일 사이즈는 기본적으로 50으로 설정.
            // 가운데로 이동시켜야 함.
            AttackEditorTileView tileView = new AttackEditorTileView(pos, unit);
            float tileSize = 50;
            Vector2 startPos = new Vector2((viewSize.x - MAP_WIDTH * tileSize) / 2, (viewSize.y - MAP_HEIGHT * tileSize) / 2);

            tileView.style.position = Position.Absolute;
            tileView.style.left = startPos.x + pos.x * tileSize;
            tileView.style.top = startPos.y + (MAP_HEIGHT - pos.y) * tileSize; // 맵 배치 특성상 상하는 역방향으로

            // 선택한 곳은 즉각적으로 변경
            // 드래그 되는 곳은 startBrusing을 체크한 뒤 변경.
            tileView.RegisterCallback<PointerDownEvent>(evt =>
            {
                // 좌클릭 우클릭에 따라 선택의 유무가 변함.
                AttackEditorTileView.SelectState state = AttackEditorTileView.SelectState.SELECTED;
                tileEditorView.Select(pos);
                if (evt.button == (int)MouseButton.RightMouse)
                {
                    // Attack에 있는 정보도 날려줌.
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