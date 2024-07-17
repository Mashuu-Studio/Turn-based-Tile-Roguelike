using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    #region Instance
    public static DragController Instance { get { return instance; } }
    private static DragController instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    private static UnitObject draggingUnit;
    private bool dragging;

    public void StartDrag(UnitObject unit)
    {
        BoardController.Instance.ShowUnitArea(true, false);
        draggingUnit = unit;
        draggingUnit.Dragging(true);

        dragging = true;
    }

    private void EndDrag()
    {
        BoardController.Instance.ShowUnitArea(false, false);
        draggingUnit.Dragging(false);
        draggingUnit = null;

        dragging = false;
    }

    private void Update()
    {
        if (GameController.Instance.IsStart) return;

        var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        point.z = 0;
        if (Input.GetMouseButtonDown(0))
        {
            var hit = Physics2D.OverlapPoint(point, LayerMask.GetMask("Unit"));
            if (hit != null)
            {
                var unit = hit.GetComponent<UnitObject>();
                // �� ������ ��쿡�� �巡���� �� ����.
                if (!unit.IsEnemy) StartDrag(unit);
            }
        }

        if (dragging && Input.GetMouseButtonUp(0))
        {
            var hit = Physics2D.OverlapPoint(point, LayerMask.GetMask("Board"));
            if (hit != null)
            {
                var board = hit.GetComponent<Board>();
                // ���� �ȿ� ���� ���� �巡�� ����
                if (BoardController.Instance.UnitOnArea(board.Pos, draggingUnit.IsEnemy))
                    BoardController.Instance.SetUnit(board, draggingUnit);
            }
            EndDrag();
        }
    }
}
