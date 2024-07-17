using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public static BoardController Instance { get { return instance; } }
    private static BoardController instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        SetBoard(10, 5);
    }

    [SerializeField] private Board boardPrefab;
    public int Width { get { return width; } }
    public int Height { get { return height; } }
    private int width, height;
    private Board[,] boards = new Board[1, 1];

    public void SetBoard(int w, int h)
    {
        boardPrefab.gameObject.SetActive(false);

        width = w;
        height = h;
        Astar.SetMapSize(w, h);

        foreach (var board in boards)
        {
            if (board != null) Destroy(board.gameObject);
        }

        boards = new Board[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Board board = Instantiate(boardPrefab, transform);
                float xpos = (width - 1) / 2f - (width - 1 - x);
                float ypos = (height - 1) / 2f - (height - 1 - y);
                board.transform.localPosition = new Vector2(xpos, ypos);
                board.gameObject.SetActive(true);
                board.SetBoard(x, y);

                boards[x, y] = board;
            }
        }
    }

    private List<Vector2Int> showingAttackRange = new List<Vector2Int>();
    public void ShowAttackRange(List<Vector2Int> range)
    {
        if (showingUnitArea) return;
        showingAttackRange.ForEach(pos => boards[pos.x, pos.y].ShowAttackRange(false));
        showingAttackRange.Clear();

        if (range != null)
        {
            showingAttackRange.AddRange(range);
            showingAttackRange.ForEach(pos => boards[pos.x, pos.y].ShowAttackRange(true));
        }
    }

    private bool showingUnitArea;
    public void ShowUnitArea(bool b, bool isEnemy)
    {
        showingUnitArea = b;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!UnitOnArea(new Vector2Int(x, y), isEnemy))
                {
                    boards[x, y].ShowNotAllowedBoard(b);
                }
                else
                {
                    boards[x, y].ShowNotAllowedBoard(false);
                }
            }
        }
    }

    #region Utils

    public int[,] ConvertToInt(bool isEnemy)
    {
        int[,] info = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // 유닛이 존재하고 같은 편이라면 벽으로 취급
                if (boards[x, y].HasUnit && boards[x, y].Unit.IsEnemy == isEnemy)
                {
                    info[x, y] = Astar.WALL;
                }
            }
        }
        return info;
    }

    public Vector2Int SelectRandomEmptyBoard(bool isEnemy)
    {
        Vector2Int pos = Vector2Int.zero;
        do
        {
            pos.x = Random.Range(0, Width);
            pos.y = Random.Range(0, Height);
        } while (BoardOnUnit(pos) || !UnitOnArea(pos, isEnemy));

        return pos;
    }

    public Vector3 WorldPointOfBoard(Vector2Int pos)
    {
        return boards[pos.x, pos.y].transform.position;
    }

    public bool UnitOnArea(Vector2Int pos, bool isEnemy)
    {
        // 홀수의 경우 가운데영역을 공유함
        if (Width % 2 == 1)
            return (isEnemy && pos.x >= Width / 2)
                || (!isEnemy && pos.x <= Width / 2);
        else
            return (isEnemy && pos.x >= Width / 2)
            || (!isEnemy && pos.x < Width / 2);
    }

    public bool BoardAvailiable(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < Width && pos.y >= 0 && pos.y < Height;
    }

    public bool BoardOnUnit(Vector2Int pos)
    {
        if (!BoardAvailiable(pos)) return false;

        return boards[pos.x, pos.y].HasUnit;
    }

    public bool BoardOnEnemy(bool isEnemy, Vector2Int pos)
    {
        // 보드에 유닛이 있으면서 팀이 달라야 함.
        return BoardOnUnit(pos) && isEnemy != boards[pos.x, pos.y].Unit.IsEnemy;
    }
    #endregion

    #region Unit Management
    public void SetUnit(Board board, UnitObject unit)
    {
        if (board.HasUnit) return;
        MoveUnit(unit, board.Pos);
    }

    public void MoveUnit(UnitObject unit, Vector2Int pos)
    {
        if (!BoardAvailiable(pos)) return;

        boards[unit.pos.x, unit.pos.y].SetUnit(null);
        boards[pos.x, pos.y].SetUnit(unit);

        unit.SetPos(pos);
        unit.transform.SetParent(boards[pos.x, pos.y].transform);
        unit.transform.localPosition = Vector3.zero;
    }

    public void DamageUnit(Vector2Int pos, int dmg)
    {
        boards[pos.x, pos.y].Damaged(dmg);
    }

    public void RemoveUnit(Vector2Int pos)
    {
        boards[pos.x, pos.y].SetUnit(null);
    }
    #endregion

    #region Gizmo
#if UNITY_EDITOR
    public string[,] astarResult;
    public void SetAstarResult(Astar.AstarNode[,] nodes)
    {
        astarResult = new string[nodes.GetLength(0), nodes.GetLength(1)];

        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                astarResult[i, j] = $"G:{nodes[i, j].G},H:{nodes[i, j].H}";
            }
        }

    }
    List<Vector2Int> path = new List<Vector2Int>();
    public void SetRoad(Stack<Vector2Int> path, Vector2Int pos)
    {
        this.path.Clear();
        if (path != null)
        {
            this.path.Add(pos);
            this.path.AddRange(path);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 1; i < path.Count; i++)
        {
            Gizmos.DrawLine(boards[path[i].x, path[i].y].transform.position,
                boards[path[i - 1].x, path[i - 1].y].transform.position);
        }

        if (astarResult != null)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 20;
            for (int i = 0; i < astarResult.GetLength(0); i++)
            {
                for (int j = 0; j < astarResult.GetLength(1); j++)
                {
                    UnityEditor.Handles.Label(boards[i, j].transform.position, astarResult[i, j], style);
                }
            }
        }
    }
#endif
    #endregion
}
