using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MapController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<Grid>();
        MapObject.Create(Resources.Load<Map>("New Map")).transform.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
