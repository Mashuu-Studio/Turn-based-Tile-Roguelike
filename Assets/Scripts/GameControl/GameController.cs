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
    [SerializeField] private PlayerObject player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) Next(Vector3Int.right);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) Next(Vector3Int.left);
        else if (Input.GetKeyDown(KeyCode.UpArrow)) Next(Vector3Int.up);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) Next(Vector3Int.down);
    }

    // 행동을 진행하면 UnitController에서 행동 활성화
    // 다음 행동을 할 때까지 대기.
    // 체크용 턴 넘기기

    public void StartGame(int seed)
    {
        this.seed = seed;
        Random.InitState(seed);
    }

    private void Next(Vector3Int dir)
    {
        bool moveMap = StageController.Instance.Move(player.Pos, dir);
        if (moveMap
            || StageController.Instance.CurrentMap.Available(player.Pos + dir))
        {
            // 맵 밖에 못 나가게 하는 작업도 필요
            if (!moveMap) player.Move(dir);
            UnitController.Instance.ActivateUnits();
        }
    }

    // 현재 Player를 관리하고 있어서 사용. 나중에 독립.
    public void Damaged(int dmg)
    {
        player.Damaged(dmg);
    }

    // 현재 Player를 관리하고 있어서 사용. 나중에 독립.
    public void SetPlayer(Vector3Int pos)
    {
        player.transform.parent = StageController.Instance.CurrentMap.transform;
        player.SetPos(pos);
    }

    public Vector3Int PlayerPos { get { return player.Pos; } }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "NEW MAP"))
        {
            StartGame(seed);
            StageController.Instance.CreateStage(1);
        }
    }
}
