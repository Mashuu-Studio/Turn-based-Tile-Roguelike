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


    [SerializeField] PlayerObject player;
    private void Start()
    {
        // Start 순서가 안 꼬이게 하기 위한 작업.
        MapController.Instance.gameObject.AddComponent<Grid>();
        var map = MapObject.Create(Resources.Load<Map>("New Map"));
        map.transform.parent = MapController.Instance.transform;
        MapController.Instance.SetMap(map);

        // 같은 맵에 두기 위한 작업. 나중에는 독립.
        player.transform.parent = MapController.Instance.CurrentMap.transform;
        player.SetPos(Vector3Int.zero);
    }
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
    private void Next(Vector3Int dir)
    {
        // 맵 밖에 못 나가게 하는 작업도 필요`
        player.Move(dir);
        UnitController.Instance.ActivateUnits();
    }

    // 현재 Player를 관리하고 있어서 사용. 나중에 독립.
    public void Damaged(int dmg)
    {
        player.Damaged(dmg);
    }

    public Vector3Int PlayerPos { get { return player.Pos; } }
}
