using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    #region Instance
    public static UIController Instance { get { return instance; } }
    private static UIController instance;
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

    // 임시 승 패 구분 세팅
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject lose;
    [SerializeField] private GameObject start;
    [SerializeField] private GameObject reset;

    [Space]
    [SerializeField] private List<UnitUI> unitUIs;
    [SerializeField] private List<UnitUI> enemyUIs;

    [Space]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private List<UnitTurnUI> unitTurnUIs;
    private int turnIndex;

    #region Init Game
    public void SetUnitInfo(List<UnitObject> units)
    {
        int unitIndex = 0;
        int enemyIndex = 0;
        foreach (var unit in units)
        {
            if (unit.IsEnemy) enemyUIs[enemyIndex++].SetUnit(unit);
            else unitUIs[unitIndex++].SetUnit(unit);
        }
        for (int i = unitIndex; i < unitUIs.Count; i++)
        {
            unitUIs[i].gameObject.SetActive(false);
        }

        for (int i = enemyIndex; i < enemyUIs.Count; i++)
        {
            enemyUIs[i].gameObject.SetActive(false);
        }
    }

    public void ResetGame()
    {
        win.SetActive(false);
        lose.SetActive(false);
        start.SetActive(true);
        unitTurnUIs.ForEach(u => u.gameObject.SetActive(false));
        SetTime(GameController.GAME_TIME);
    }

    public void StartGame()
    {
        win.SetActive(false);
        lose.SetActive(false);
        start.SetActive(false);
        reset.SetActive(false);

        turnIndex = 0;
        SetTurnList();
    }
    #endregion

    #region Progress Game
    public void SetTime(float time)
    {
        timeText.text = time.ToString();
    }

    public void SetTurnList()
    {
        for (int i = 0; i < unitTurnUIs.Count; i++)
        {
            int turn = turnIndex + i;
            if (turn >= unitTurnUIs.Count) turn -= unitTurnUIs.Count;
            unitTurnUIs[turn].SetTurn(UnitController.Instance.GetUnitNextTurn(i));
            unitTurnUIs[turn].gameObject.SetActive(true);
        }
    }

    public void ProgressTurn()
    {
        // 액션이 끝난 뒤에 남은 행동이 없다면 다음 턴 유닛을 세팅해주어야 함.
        if (!unitTurnUIs[turnIndex].Action())
        {
            // 먼저 가장 위에 있던 오브젝트를 가장 아래로 내려주고
            // UnitController로부터 표기 상으로 가장 마지막에 행동을 해야할 유닛을 받아온 뒤 세팅해줌.
            // 이 후 turnIndex를 조절해주어 다음 유닛을 가리키도록 함.
            unitTurnUIs[turnIndex].SetTurn(UnitController.Instance.GetUnitNextTurn(unitTurnUIs.Count - 1));
            unitTurnUIs[turnIndex].transform.SetAsLastSibling();
            turnIndex++;
            if (turnIndex >= unitTurnUIs.Count) turnIndex = 0;
        }
    }

    public void GameOver(bool win)
    {
        this.win.SetActive(win);
        lose.SetActive(!win);
        reset.SetActive(true);
    }
    #endregion
}
