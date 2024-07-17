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

    private List<UnitObject> units = new List<UnitObject>();
    public int UnitAmount { get { return unitAmount; } }
    public int EnemyAmount { get { return enemyAmount; } }
    private int unitAmount;
    private int enemyAmount;

    public void Init()
    {
        units.Clear();
        AddUnit((UnitObject)PoolController.Pop("UNIT.MELEE"), BoardController.Instance.SelectRandomEmptyBoard(false), false);
        AddUnit((UnitObject)PoolController.Pop("UNIT.MELEERANGE"), BoardController.Instance.SelectRandomEmptyBoard(false), false);
        AddUnit((UnitObject)PoolController.Pop("UNIT.RANGE"), BoardController.Instance.SelectRandomEmptyBoard(false), false);
        AddUnit((UnitObject)PoolController.Pop("UNIT.MELEE"), BoardController.Instance.SelectRandomEmptyBoard(true), true);
        AddUnit((UnitObject)PoolController.Pop("UNIT.MELEERANGE"), BoardController.Instance.SelectRandomEmptyBoard(true), true);
        AddUnit((UnitObject)PoolController.Pop("UNIT.RANGE"), BoardController.Instance.SelectRandomEmptyBoard(true), true);
        UIController.Instance.SetUnitInfo(units);
    }

    public void StartGame()
    {
        orderdedUnits.Clear();
        orderdedUnits.AddRange(units);
        orderdedUnits.Sort(CompareUnitSpeed);
        unitTurnIndex = 0;
    }

    public List<Vector2Int> EnemyPositions(bool isEnemy)
    {
        List<Vector2Int> targets = new List<Vector2Int>();
        foreach (var unit in units)
        {
            if (unit.IsEnemy != isEnemy) targets.Add(unit.pos);
        }
        return targets;
    }

    #region Adjust Unit
    public void AddUnit(UnitObject unit, Vector2Int pos, bool isEnemy)
    {
        unit.SetUnit(isEnemy);
        BoardController.Instance.MoveUnit(unit, pos);

        units.Add(unit);
        if (isEnemy) enemyAmount++;
        else unitAmount++;
    }

    private int CompareUnitSpeed(UnitObject unit1, UnitObject unit2)
    {
        return unit2.Speed - unit1.Speed;
    }

    public void ResetUnits()
    {
        while (units.Count > 0) units[0].Dead();
        Init();
    }

    public void RemoveUnit(UnitObject unit)
    {
        units.Remove(unit);
        CheckRemainUnitAmount();

        int index = orderdedUnits.FindIndex(u => u == unit);
        if (index >= 0)
        {
            /*
            if (unit.IsEnemy) enemyAmount--;
            else unitAmount--;
            */
            orderdedUnits.RemoveAt(index);
            // 만약에 죽은 유닛이 앞 순번이라면 턴index를 앞으로 당겨줌
            // 또는, 유닛이 죽음으로 인해 unitTurnIndex가 넘어가게 되면 0으로 초기화해줌.
            if (index < unitTurnIndex) unitTurnIndex--;
            if (unitTurnIndex >= units.Count) unitTurnIndex = 0;

            // 유닛이 죽었다면 순번이 틀어지기 때문에 다시 세팅해주어야함.
            if (orderdedUnits.Count > 0) UIController.Instance.SetTurnList();
        }
    }

    private void CheckRemainUnitAmount()
    {
        int u = 0, e = 0;
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].IsEnemy) e++;
            else u++;
        }
        unitAmount = u;
        enemyAmount = e;
    }
    #endregion

    #region InGame

    // UI에 표기되는 유닛의 순서와 속도에 따라 정렬되는 유닛의 순서를 다르게 관리.
    private List<UnitObject> orderdedUnits = new List<UnitObject>();
    private int unitTurnIndex;
    public void ActivateUnit()
    {
        var unit = orderdedUnits[unitTurnIndex];
        // 행동이 남았다면 다음 유닛의 차례로 넘어가지 않음.
        // 마지막 행동이라면 다음 유닛의 차례로 넘어감.
        if (unit.RemainAction == 1)
        {
            unitTurnIndex++;
            // 마지막 유닛으로 왔다면 처음으로
            if (unitTurnIndex >= units.Count) unitTurnIndex = 0;
        }
        unit.Action();
        // 범위 내에 적이 있다면 공격 없다면 이동
        if (unit.SearchTargets.Count > 0) unit.Attack();
        else unit.Move();
    }

    public UnitObject GetUnitNextTurn(int turn)
    {
        if (orderdedUnits.Count == 0) return null;
        // 현재 턴에서 특정 턴만큼 미리 이동
        turn = unitTurnIndex + turn;
        while (turn >= orderdedUnits.Count) turn -= orderdedUnits.Count;
        return orderdedUnits[turn];
    }

    public void GameOver()
    {

    }
    #endregion
}
