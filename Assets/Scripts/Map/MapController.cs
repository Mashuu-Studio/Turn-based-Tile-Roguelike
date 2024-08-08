using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public static MapController Instance { get { return instance; } }
    private static MapController instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public MapObject CurrentMap { get { return currentMap; } }
    private MapObject currentMap;

    public void SetMap(MapObject map)
    {
        currentMap = map;
        Camera.main.transform.position = currentMap.center;
    }
}
