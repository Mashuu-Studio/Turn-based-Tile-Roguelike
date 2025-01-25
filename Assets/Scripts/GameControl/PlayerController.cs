using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Instance
    public static PlayerController Instance { get { return instance; } }
    private static PlayerController instance;
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

    [SerializeField] private PlayerObject player;
    private Vector2Int viewDirection = Vector2Int.zero;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) Attack();
        else if (Input.GetButtonDown("Right")) Move(Vector3Int.right);
        else if (Input.GetButtonDown("Left")) Move(Vector3Int.left);
        else if (Input.GetButtonDown("Up")) Move(Vector3Int.up);
        else if (Input.GetButtonDown("Down")) Move(Vector3Int.down);

        var dir = GetPlayerDirection();
        if (viewDirection != dir)
        {
            viewDirection = dir;
            ShowPlayerRange();
        }

    }

    public void ShowPlayerRange()
    {
        StageController.Instance.CurrentMap.ClearRange();
        StageController.Instance.CurrentMap.PlayerRange(player.Pos, Inventory.EquipCard.style.attackRange, viewDirection);
    }

    public Vector2Int GetPlayerDirection()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var watch = mousePos - player.transform.position;
        if (watch.x > 0)
        {
            if (watch.y > watch.x) return Vector2Int.up;
            else if (watch.y * -1 > watch.x) return Vector2Int.down;
            else return Vector2Int.right;
        }
        else
        {
            if (watch.y > watch.x * -1) return Vector2Int.up;
            else if (watch.y * -1 > watch.x * -1) return Vector2Int.down;
            else return Vector2Int.left;
        }
    }

    private void Move(Vector3Int dir)
    {
        bool moveMap = StageController.Instance.Move(player.Pos, dir);
        if (moveMap
            || StageController.Instance.CurrentMap.Available(player.Pos + dir))
        {
            // 맵 밖에 못 나가게 하는 작업도 필요
            if (!moveMap) player.Move(dir);
            ShowPlayerRange();
            GameController.Instance.NextTurn();
        }
    }

    public void Attack()
    {
        Debug.Log("Attack");
        GameController.Instance.NextTurn();
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
}
