using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    #region Instance
    public static UnitController Instance { get { return instance; } }
    private static UnitController instance;
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

    // ���� ���� ����. width, height ��.
    List<UnitObject> units;

    // ���� ���� ������ �߰���ų �Լ�
    // �ʿ��� ����.
    public void AddUnits(List<UnitObject> units)
    {
        this.units = units;
    }

    // ������ ������ �Լ�
    public void DeadUnit(UnitObject unit)
    {
        // ���� ������ �� ������Ʈ������ �ش� ������ ������ �˷��� ��.
        // Ȥ�� Ŭ���� ���� �� �˸��� ��.
    }

    // ���� �����ų �Լ� -> ��ü ������ ���������� �ൿ�ؾ���.
    // �÷��̾ �ൿ�� �����ϸ� �̾ ���޾� ����.
    Vector3Int[] dirs = new Vector3Int[4]
    {
        Vector3Int.left,
        Vector3Int.right,
        Vector3Int.up,
        Vector3Int.down
    };
    public void ActivateUnits()
    {
        StageController.Instance.CurrentMap.ClearRange();
        // �켱 �������� �۵�. 
        foreach (var unit in units)
        {
            var rand = Random.Range(0, 2);
            if (rand == 0)
            {
                // ���� �̵��� ���� ����. ���߿��� Astar�� �ڸ��� ����.
                Vector3Int dir;
                Vector3Int pos;
                do
                {
                    int r = Random.Range(0, dirs.Length);
                    dir = dirs[r];
                    pos = unit.Pos + dir;
                } while (StageController.Instance.CurrentMap.Available(pos));
                unit.Move(dir);
            }
            else
            {
                unit.Attack();
            }
        }
    }
}
