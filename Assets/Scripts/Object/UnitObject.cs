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
        // ���� Parent�� ����������. ���⿡ ������ �ٸ� ���� �������� ���.
        this.data = data;
        this.pos = pos;
        transform.localPosition = new Vector3(pos.x, pos.y);

        hp = data.hp;
        dmg = data.dmg;

        // �ӽÿ� ����. 
        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Viking");
        spriteRenderer.color = Color.red;
        return this;
    }

    // �ʿ� ���ӽ��� localposition�� Ȱ���ϸ� ���� �� ��.
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
