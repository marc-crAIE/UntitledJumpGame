using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlatformSpawner : MonoBehaviour
{
    public Camera camera;
    public GameObject platform;

    private float _areaWidth;
    private float _areaHeight;

    private int _gridHeight;

    // Start is called before the first frame update
    void Start()
    {
        // Get the height of the view area
        float distance = Vector3.Distance(this.transform.position, camera.transform.position);
        Vector3 viewBottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 viewTopRight = camera.ViewportToWorldPoint(new Vector3(1, 1, distance));
        
        // Calculate the amount of platforms to have in that area
        _areaWidth = viewTopRight.x - viewBottomLeft.x;
        _areaHeight = viewTopRight.y - viewBottomLeft.y;
        _gridHeight = (int)(_areaHeight / platform.transform.localScale.y);
        
        // Spawn them
        for (int gridY = 0; gridY < _gridHeight; gridY++)
        {
            float posX = Random.Range(0, _areaWidth) - (_areaWidth / 2.0f);
            float posY = gridY - (_gridHeight / 2.0f);
            SpawnPlatform(posX, posY);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnPlatform(float x, float y)
    {
        Instantiate(platform, new Vector3(x, y, 0), Quaternion.identity, transform);
    }
}
