using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Board boardPrefab;
    [SerializeField] private Vector2Int halfsize;

    private Board[,] boards;

    private void Start()
    {
        boards = new Board[halfsize.x * 2 + 1, halfsize.y * 2 + 1];
        for (int i = -halfsize.x; i <= halfsize.x; i++)
        {
            for (int j = -halfsize.y; j <= halfsize.y; j++)
            {
                boards[i + halfsize.x, j + halfsize.y] = Instantiate(boardPrefab, transform);
                boards[i + halfsize.x, j + halfsize.y].transform.position = new Vector2(i, j);
            }
        }
        boardPrefab.gameObject.SetActive(false);
    }

}
