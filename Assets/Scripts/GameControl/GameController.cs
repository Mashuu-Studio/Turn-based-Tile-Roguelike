using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Instance
    public static GameController Instance { get { return instance; } }
    private static GameController instance;
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

    public bool IsStart { get { return isStart; } }
    private bool isStart;

    public const int GAME_TIME = 60;
    private int totalTime;

    private IEnumerator gameCoroutine;
   
    public void ResetGame()
    {
        UnitController.Instance.ResetUnits();
        UIController.Instance.ResetGame();
    }

    public void StartGame()
    {
        isStart = true;
        UnitController.Instance.StartGame();
        UIController.Instance.StartGame();
        gameCoroutine = ProgressGame();
        StartCoroutine(gameCoroutine);
    }

    public void GameOver(bool win)
    {
        UnitController.Instance.GameOver();
        UIController.Instance.GameOver(win);
        isStart = false;
    }

    private IEnumerator ProgressGame()
    {
        totalTime = GAME_TIME;
        float time = 0;
        while (totalTime > 0
            && UnitController.Instance.UnitAmount > 0
            && UnitController.Instance.EnemyAmount > 0)
        {
            time += Time.deltaTime;
            yield return null;
            if (time > 1f)
            {
                time--;
                totalTime--;
                UnitController.Instance.ActivateUnit();
                UIController.Instance.SetTime(totalTime);
                UIController.Instance.ProgressTurn();
            }
        }

        if (UnitController.Instance.EnemyAmount == 0) GameOver(true);
        if (UnitController.Instance.UnitAmount == 0) GameOver(false);
    }
}
