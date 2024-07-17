using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolController : MonoBehaviour
{
    public static PoolController Instance { get { return instance; } }
    private static PoolController instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private static string path = "Poolable";
    private Dictionary<string, Pool> pools;

    public void Init()
    {
        pools = new Dictionary<string, Pool>();
        Poolable[] poolables = Resources.LoadAll<Poolable>(path);
        foreach (var poolable in poolables)
        {
            CreatePool(poolable);
        }
    }

    public static void CreatePool(Poolable poolable)
    {
        if (Instance.pools.ContainsKey(poolable.name)) return;

        GameObject go = new GameObject(poolable.name);
        go.transform.SetParent(Instance.transform);
        Pool pool = go.AddComponent<Pool>();

        pool.Init(poolable);
        Instance.pools.Add(poolable.name, pool);
    }

    public static void Push(string name, Poolable poolable)
    {
        if (Instance.pools.ContainsKey(name))
        {
            Instance.pools[name].Push(poolable);
        }
        else
        {
            CreatePool(poolable);
            Instance.pools[name].Push(poolable);
        }
    }

    public static Poolable Pop(string name)
    {
        if (Instance.pools.ContainsKey(name)) return Instance.pools[name].Pop();
        else return null;
    }
}
