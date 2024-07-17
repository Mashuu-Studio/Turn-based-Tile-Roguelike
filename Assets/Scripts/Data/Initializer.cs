using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    private void Start()
    {
        SpriteManager.Init();
        UnitManager.Init();
        PoolController.Instance.Init();
        GameController.Instance.ResetGame();
    }
}
