using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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


    public int seed;

    // �ൿ�� �����ϸ� UnitController���� �ൿ Ȱ��ȭ
    // ���� �ൿ�� �� ������ ���.
    // üũ�� �� �ѱ��

    private void Start()
    {
        StartGame(seed);
        StageController.Instance.CreateStage(1);
    }

    public void StartGame(int seed)
    {
        this.seed = seed;
        Random.InitState(seed);
    }

    public void NextTurn()
    {
        UnitController.Instance.ActivateUnits();
        Debug.Log("Next");
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "NEW MAP"))
        {
            StartGame(seed);
            StageController.Instance.CreateStage(1);
        }
    }
}
