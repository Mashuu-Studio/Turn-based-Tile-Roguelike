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
    [SerializeField] private UnitObject unit;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) unit.Move(Vector2Int.right);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) unit.Move(Vector2Int.left);
        else if (Input.GetKeyDown(KeyCode.UpArrow)) unit.Move(Vector2Int.up);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) unit.Move(Vector2Int.down);
    }
}
