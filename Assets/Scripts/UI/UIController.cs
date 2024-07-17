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

    // �ӽ� �� �� ���� ����
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
        // �׼��� ���� �ڿ� ���� �ൿ�� ���ٸ� ���� �� ������ �������־�� ��.
        if (!unitTurnUIs[turnIndex].Action())
        {
            // ���� ���� ���� �ִ� ������Ʈ�� ���� �Ʒ��� �����ְ�
            // UnitController�κ��� ǥ�� ������ ���� �������� �ൿ�� �ؾ��� ������ �޾ƿ� �� ��������.
            // �� �� turnIndex�� �������־� ���� ������ ����Ű���� ��.
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
