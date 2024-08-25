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

        // View 초기화
        internal void PopulateView(Attack attack)
        {
            this.attack = attack;
            this.attack.OnChanged += DrawView;
            //this.map.LoadMap();
            // View의 크기가 바뀌면 가운데로 위치 조정 다시 해줌.
            RegisterCallback<GeometryChangedEvent>(evt => SetView());

            // Brush질을 진행할 수 있도록 해당 뷰를 선택해도 시작을 걸어줌.
            RegisterCallback<PointerDownEvent>(evt => startBrushing = true);
            RegisterCallback<PointerUpEvent>(evt => startBrushing = false);
            RegisterCallback<MouseLeaveEvent>(evt => startBrushing = false);

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
                    CreateTileView(pos, viewSize, pos == center, attack.attackRange.Contains(pos - center));
                }
            }
        }

        // 현 상태를 건드리지 않은 상태로 색만 변경
        public void DrawView()
        {
            bool prev = brushSelected;
            // 전부 미선택상태로
            brushSelected = false;
            foreach (var tile in tiles)
            {
                TileChanged(tile.pos, false);
            }

            // range에 있는 부분만 색 추가
            brushSelected = true;
            foreach (var pos in attack.attackRange)
            {
                TileChanged(pos + center, false);
            }

            brushSelected = prev;
        }

        // 타일 생성
        private void CreateTileView(Vector3Int pos, Vector2 viewSize, bool unit, bool selected)
        {
            // 좌하단을 0,0으로 시작하여 우상단을 MAP_WIDTH-1,MAP_HEIGHT-1로 마무리.
            // 타일 사이즈는 기본적으로 50으로 설정.
            // 가운데로 이동시켜야 함.
            AttackEditorTileView tileView = new AttackEditorTileView(pos, unit, selected);
            float tileSize = 50;
            Vector2 startPos = new Vector2((viewSize.x - MAP_WIDTH * tileSize) / 2, (viewSize.y - MAP_HEIGHT * tileSize) / 2);
           
            tileView.style.position = Position.Absolute;
            tileView.style.left = startPos.x + pos.x * tileSize;
            tileView.style.top = startPos.y + pos.y * tileSize;

            // 선택한 곳은 즉각적으로 변경
            // 드래그 되는 곳은 startBrusing을 체크한 뒤 변경.
            tileView.RegisterCallback<PointerDownEvent>(evt =>
            {
                // 좌클릭 우클릭에 따라 선택의 유무가 변함.
                brushSelected = evt.button == (int)MouseButton.LeftMouse;
                TileChanged(pos);
            });
            tileView.RegisterCallback<PointerMoveEvent>(evt =>
            {
                if (startBrushing)
                {
                    // 중간에 클릭이 변경되면 인식. On Off를 바꿈.
                    if (brushSelected && Event.current.type == EventType.MouseDown && Event.current.button == 1) brushSelected = false;
                    else if (!brushSelected && Event.current.type == EventType.MouseDown && Event.current.button == 0) brushSelected = true;
                    
                    // 만약에 양클릭으로 진행되다 한 쪽의 버튼이 풀리게 되면 이 역시 인식. On Off 변경.
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
            // 리드로우의 경우에는 추가하지 않도록.
            if (add) attack.AddRange(pos - center, tiles[pos.x, pos.y].selected);
            /*
            bool horizontal = tilePalette.GetSupportActivate(TilePaletteView.SupportItemType.HORIZONTAL_SYMMETRY);
            bool vertical = tilePalette.GetSupportActivate(TilePaletteView.SupportItemType.VERTICAL_SYMMETRY);

            int x_inverse = map.MAP_WIDTH - 1 - pos.x;
            int y_inverse = map.MAP_HEIGHT - 1 - pos.y;
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