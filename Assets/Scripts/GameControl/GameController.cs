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
        // Start ������ �� ���̰� �ϱ� ���� �۾�.
        MapController.Instance.gameObject.AddComponent<Grid>();
        var map = MapObject.Create(Resources.Load<Map>("New Map"));
        map.transform.parent = MapController.Instance.transform;
        MapController.Instance.SetMap(map);

        // ���� �ʿ� �α� ���� �۾�. ���߿��� ����.
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

    // �ൿ�� �����ϸ� UnitController���� �ൿ Ȱ��ȭ
    // ���� �ൿ�� �� ������ ���.
    // üũ�� �� �ѱ��
    private void Next(Vector3Int dir)
    {
        // �� �ۿ� �� ������ �ϴ� �۾��� �ʿ�`
        player.Move(dir);
        UnitController.Instance.ActivateUnits();
    }

    // ���� Player�� �����ϰ� �־ ���. ���߿� ����.
    public void Damaged(int dmg)
    {
        player.Damaged(dmg);
    }

    public Vector3Int PlayerPos { get { return player.Pos; } }
}
