using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class Board : MonoBehaviour
{
    public Vector2Int Pos { get { return pos; } }
    private Vector2Int pos;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetBoard(int x, int y, Sprite sprite)
    {
        pos = new Vector2Int(x, y);
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
