using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class Board : MonoBehaviour
{
    public bool HasUnit { get { return unit != null; } }
    public UnitObject Unit { get { return unit; } }
    public Vector2Int Pos { get { return pos; } }
    private Vector2Int pos;
    private UnitObject unit;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetBoard(int x, int y)
    {
        pos = new Vector2Int(x, y);
    }

    public void SetUnit(UnitObject unit)
    {
        this.unit = unit;
    }

    public void Damaged(int dmg)
    {
        if (unit != null)
        {
            unit.Damaged(dmg);
        }
    }

    public void ShowAttackRange(bool b)
    {
        if (b) spriteRenderer.color = Color.green;
        else spriteRenderer.color = Color.white;
    }

    public void ShowNotAllowedBoard(bool b)
    {
        if (b) spriteRenderer.color = Color.red;
        else spriteRenderer.color = Color.white;
    }
}
