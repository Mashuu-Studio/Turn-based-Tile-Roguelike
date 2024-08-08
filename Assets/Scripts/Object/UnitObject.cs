using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UnitObject : Poolable
{
    public Unit Data { get { return data; } }
    protected Unit data;

    public Vector3Int Pos { get { return pos; } }
    protected Vector3Int pos;
    protected int hp;
    protected int dmg;

    public override void Init()
    {

    }

    public UnitObject Summon(Unit data, Vector3Int pos)
    {
        // 맵을 Parent로 가져가야함. 여기에 넣을지 다른 곳에 넣을지는 고민.
        this.data = data;
        this.pos = pos;
        transform.localPosition = new Vector3(pos.x, pos.y);

        hp = data.hp;
        dmg = data.dmg;

        // 임시용 세팅. 
        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Viking");
        spriteRenderer.color = Color.red;
        return this;
    }

    // 맵에 종속시켜 localposition을 활용하면 좋을 듯 함.
    public void Move(Vector3Int dir)
    {
        transform.localPosition += new Vector3(dir.x, dir.y);
        pos += dir;
    }

    public abstract void Attack();

    public abstract void Dead();
    public void Damaged(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            hp = 0;
        }
        Debug.Log($"{gameObject.name} HP : {hp}");
    }
}
