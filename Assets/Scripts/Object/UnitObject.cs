using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class UnitObject : Poolable
{

    public Vector2Int pos;

    public void Move(Vector2Int dir)
    {
        pos += dir;
        transform.position = (Vector2)pos;
    }
    /*
    [SerializeField] private string key;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public Sprite sprite { get { return spriteRenderer.sprite; } }

    public bool IsEnemy { get { return isEnemy; } }
    private bool isEnemy;
    public Unit Data { get { return data; } }
    private Unit data;

    public int Hp { get { return hp; } }
    public int Dmg { get { return dmg; } }
    public int Speed { get { return speed; } }
    public int Actions { get { return actions; } }
    public int RemainAction { get { return remainAction; } }

    private int hp;
    private int dmg;
    private int speed;
    private int actions;
    private int remainAction;
    private int range;
    public override void Init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        data = UnitManager.GetUnit(key);
    }

    public void SetUnit(bool isEnemy)
    {
        hp = data.hp;
        dmg = data.dmg;
        speed = data.speed;
        remainAction = actions = data.actions;
        range = data.range;

        this.isEnemy = isEnemy;
        spriteRenderer.flipX = isEnemy;
        spriteRenderer.color = Color.white;
        spriteRenderer.material.SetColor("_Color", isEnemy ? Color.red : Color.green);
    }

    #region Action
    public void Action()
    {
        remainAction--;
        if (remainAction <= 0) remainAction = actions;
    }

    #region Move

    public Vector2Int pos;

    private Stack<Vector2Int> path = new Stack<Vector2Int>();
    public void FindPath()
    {
        path.Clear();
        path = Astar.Search(
            BoardController.Instance.ConvertToInt(IsEnemy), pos,
            UnitController.Instance.EnemyPositions(isEnemy), range);
    }

    public void SetPos(Vector2Int pos)
    {
        this.pos = pos;
    }

    public void Move()
    {
        FindPath();
        if (path.Count == 0) return;
        StartCoroutine(Moving());
    }

    IEnumerator Moving()
    {
        // 보드 위치에서 현재 위치까지의 거리를 체크한 뒤 특정 시간동안 프레임단위로 이동시킴.
        animator.SetBool("move", true);
        Vector2Int dest = path.Pop();
        var d = (BoardController.Instance.WorldPointOfBoard(dest) - transform.position);
        float moveTime = 0.5f;
        float t = 0;
        while (t < moveTime)
        {
            var moveAmount = d * Time.deltaTime / moveTime;
            t += Time.deltaTime;
            transform.position += moveAmount;
            yield return null;
        }

        // 최종적으로 이동하는 모습이 끝나면 위치를 확실하게 조정
        BoardController.Instance.MoveUnit(this, dest);
        animator.SetBool("move", false);
    }

    #endregion

    #region Attack

    public List<Vector2Int> AttackRange
    {
        get
        {
            List<Vector2Int> list = new List<Vector2Int>();
            for (int x = -1 * range; x <= 1 * range; x++)
                for (int y = -1 * range; y <= 1 * range; y++)
                {
                    var pos = new Vector2Int(this.pos.x + x, this.pos.y + y);
                    // 본인은 뺌
                    if (x == 0 && y == 0) continue;
                    if (BoardController.Instance.BoardAvailiable(pos)) list.Add(pos);
                }

            return list;
        }
    }

    public List<Vector2Int> SearchTargets
    {
        get
        {
            List<Vector2Int> targets = new List<Vector2Int>();

            // 공격 범위 내에서 확인
            foreach (var target in AttackRange)
            {
                // 대상이 있다면 타겟에 포함.
                if (BoardController.Instance.BoardOnEnemy(isEnemy, target))
                    targets.Add(target);
            }

            return targets;
        }
    }

    public void Attack()
    {
        StartCoroutine(Attacking());
    }

    IEnumerator Attacking()
    {
        animator.SetTrigger("attack");
        // 모션이 다음 스텝으로 넘어가서 태그가 바뀌도록 1프레임 대기
        yield return null;
        List<Vector2Int> targets = SearchTargets;
        // 모션이 끝나기를 대기
        while (animator.GetCurrentAnimatorStateInfo(0).IsTag("ATTACK")) yield return null;

        // 범위공격의 경우 전부 공격
        if (data.type == Unit.Type.MELEERANGE)
        {
            foreach (var targetPos in targets)
            {
                BoardController.Instance.DamageUnit(targetPos, dmg);
            }
        }
        // 그외에는 랜덤하게 선택하여 공격
        else
        {
            // 후에는 확실한 타겟을 정해도 될 거 같고?
            int rand = Random.Range(0, targets.Count);
            if (data.type == Unit.Type.RANGE)
            {
                // 투사체 발사
                // 투사체 발사가 끝날떄까지 대기
                var proj = (Projectile)PoolController.Pop(data.key.Replace("UNIT", "PROJECTILE"));
                proj.Shoot(transform.position, BoardController.Instance.WorldPointOfBoard(targets[rand]), 30);
                // 투사체가 나오도록 한 프레임 대기
                yield return null;
                while (proj.IsShoot) yield return null;
            }
            BoardController.Instance.DamageUnit(targets[rand], dmg);
        }
    }

    IEnumerator Damaging()
    {
        float time = 0.3f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            spriteRenderer.color = new Color(1, 1 - time * 2, 1 - time * 2);
            yield return null;
        }
        spriteRenderer.color = Color.white;
    }

    public void Damaged(int dmg)
    {
        StartCoroutine(Damaging());

        hp -= dmg;
        if (hp <= 0)
        {
            hp = 0;
            Dead();
        }
    }

    public void Dead()
    {
        PoolController.Push(gameObject.name, this);
        UnitController.Instance.RemoveUnit(this);
        BoardController.Instance.RemoveUnit(pos);
    }

    #endregion
    #endregion*/
}
