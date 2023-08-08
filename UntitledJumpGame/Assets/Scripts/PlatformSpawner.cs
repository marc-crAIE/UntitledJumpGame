using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject Platform;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Platform, new Vector3(0, 0, 0), Quaternion.identity, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
