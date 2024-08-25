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
            // View�� ũ�Ⱑ �ٲ�� ����� ��ġ ���� �ٽ� ����.
            RegisterCallback<GeometryChangedEvent>(evt => SetView());
            selecetedUnit = unit;
            selectedAttack = null;
            SetView();
        }

        // View�� ��������.
        public void SetView()
        {
            // ��ü ũ���� �߾ӿ� �R.
            Clear();
            Vector2 viewSize = new Vector2(layout.width, layout.height);
            Vector3Int pos = Vector3Int.zero;
            for (int x = 0; x < AttackEditorView.MAP_WIDTH; x++)
            {
                for (int y = 0; y < AttackEditorView.MAP_HEIGHT; y++)
                {
                    // ������ �ּ�ȭ�� ���� �ݺ����� ������ ȣ���� �ƴ� ���� ����
                    pos.x = x;
                    pos.y = y;

                    // ������� ������ ������.
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

        // �� ���¸� �ǵ帮�� ���� ���·� ���� ����
        public void DrawView(Attack attack)
        {
            selectedAttack = attack;
            // ���� �̼��û��·�
            foreach (var tile in tiles)
            {
                TileChanged(tile.pos, false);
            }

            // range�� �ִ� �κи� �� �߰�
            foreach (var pos in attack.attackRange)
            {
                TileChanged(pos + center, true);
            }
        }

        // Ÿ�� ����
        private void CreateTileView(Vector3Int pos, Vector2 viewSize, bool unit)
        {
            // ���ϴ��� 0,0���� �����Ͽ� ������ width-1,height-1�� ������.
            // Ÿ�� ������� �⺻������ 50���� ����.
            // ����� �̵����Ѿ� ��.
            AttackEditorTileView tileView = new AttackEditorTileView(pos, unit, false);
            float min = viewSize.x < viewSize.y ? viewSize.x : viewSize.y;
            float tileSize = min * 0.9f / AttackEditorView.MAP_WIDTH; // ���� ũ�⿡ ���� ����.
            Vector2 startPos = new Vector2((viewSize.x - AttackEditorView.MAP_WIDTH * tileSize) / 2, (viewSize.y - AttackEditorView.MAP_HEIGHT * tileSize) / 2);

            tileView.style.width = tileView.style.height = tileSize;
            tileView.style.position = Position.Absolute;
            tileView.style.left = startPos.x + pos.x * tileSize;
            tileView.style.top = startPos.y + pos.y * tileSize;

            tiles[pos.x, pos.y] = tileView;
            Add(tileView);
        }

        private void TileChanged(Vector3Int pos, bool b)
        {
            tiles[pos.x, pos.y].TileChanged(b);
        }
    }
}