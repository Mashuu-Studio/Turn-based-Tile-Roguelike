using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using AttackEditor;
using static PlasticPipe.Server.MonitorStats;
using PlasticGui.WorkspaceWindow.Locks;

namespace UnitEditor
{
    public class UnitEditorAttackView : VisualElement
    {
        public AttackEditorTileView[,] tiles;
        private Vector3Int center;
        private Unit selecetedUnit;
        private Attack selectedAttack;

        public new class UxmlFactory : UxmlFactory<UnitEditorAttackView, UxmlTraits> { }

        public UnitEditorAttackView()
        {/*
            ClearClassList();
            AddToClassList("MapEditorView");*/
            center = new Vector3Int(AttackEditorView.MAP_WIDTH / 2, AttackEditorView.MAP_HEIGHT / 2);
            tiles = new AttackEditorTileView[AttackEditorView.MAP_WIDTH, AttackEditorView.MAP_HEIGHT];
        }

        internal void PopulateView(Unit unit)
        {
            // View의 크기가 바뀌면 가운데로 위치 조정 다시 해줌.
            RegisterCallback<GeometryChangedEvent>(evt => SetView());
            selecetedUnit = unit;
            selectedAttack = null;
            SetView();
        }

        // View를 세팅해줌.
        public void SetView()
        {
            // 전체 크기의 중앙에 둚.
            Clear();
            Vector2 viewSize = new Vector2(layout.width, layout.height);
            Vector3Int pos = Vector3Int.zero;
            for (int x = 0; x < AttackEditorView.MAP_WIDTH; x++)
            {
                for (int y = 0; y < AttackEditorView.MAP_HEIGHT; y++)
                {
                    // 가비지 최소화를 위해 반복적인 생성자 호출이 아닌 변수 변경
                    pos.x = x;
                    pos.y = y;

                    // 가운데에는 유닛이 들어가야함.
                    CreateTileView(pos, viewSize, pos == center);
                }
            }

            var button = new Button(() => AddAttack());
            var label = new Label("ADD");
            label.style.fontSize = 18;
            label.style.color = Color.black;
            label.style.alignContent = Align.Center;

            button.style.width = 150;
            button.style.height = 50;
            button.style.position = Position.Absolute;
            button.style.right = 25;
            button.style.bottom = 25;
            button.Add(label);
            Add(button);
        }

        public void AddAttack()
        {
            if (selectedAttack == null) return;
            selecetedUnit.AddAttack(selectedAttack);
        }

        // 현 상태를 건드리지 않은 상태로 색만 변경
        public void DrawView(Attack attack)
        {
            selectedAttack = attack;
            // 전부 미선택상태로
            foreach (var tile in tiles)
            {
                TileChanged(tile.pos, AttackEditorTileView.SelectState.UNSELECTED);
            }

            // 실제로 선택한 부분은 다른색.
            foreach (var pos in attack.attackRange)
            {
                // 방향으로 인해 생긴 부분 추가.
                foreach (var p in attack.GetRange(pos + center, new Vector3Int(AttackEditorView.MAP_WIDTH, AttackEditorView.MAP_HEIGHT)))
                {
                    TileChanged(p, AttackEditorTileView.SelectState.ADDED);
                }
                TileChanged(pos + center, AttackEditorTileView.SelectState.SELECTED);
            }
        }

        // 타일 생성
        private void CreateTileView(Vector3Int pos, Vector2 viewSize, bool unit)
        {
            // 좌하단을 0,0으로 시작하여 우상단을 width-1,height-1로 마무리.
            // 타일 사이즈는 기본적으로 50으로 설정.
            // 가운데로 이동시켜야 함.
            AttackEditorTileView tileView = new AttackEditorTileView(pos, unit);
            float min = viewSize.x < viewSize.y ? viewSize.x : viewSize.y;
            float tileSize = min * 0.9f / AttackEditorView.MAP_WIDTH; // 뷰의 크기에 따라 조정.
            Vector2 startPos = new Vector2((viewSize.x - AttackEditorView.MAP_WIDTH * tileSize) / 2, (viewSize.y - AttackEditorView.MAP_HEIGHT * tileSize) / 2);

            tileView.style.width = tileView.style.height = tileSize;
            tileView.style.position = Position.Absolute;
            tileView.style.left = startPos.x + pos.x * tileSize;
            tileView.style.top = startPos.y + (AttackEditorView.MAP_HEIGHT - pos.y) * tileSize; // 맵 배치 특성상 상하는 역방향으로

            tiles[pos.x, pos.y] = tileView;
            Add(tileView);
        }

        private void TileChanged(Vector3Int pos, AttackEditorTileView.SelectState state)
        {
            tiles[pos.x, pos.y].TileChanged(state);
        }
    }
}