using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlatformSpawnerScript : MonoBehaviour
{
    public Camera camera;
    public GameObject platform;
    public float spawnChance;
    // The max number of platform spaces between each spawned platform
    public int maxPlatformDistance;

    private float _areaWidth;
    private float _areaHeight;
    private int _gridHeight;
    private float _gridUnitHeight;
    private int _numberOfPlatforms;

    private Vector3 _prevSpawnCheckPos;
    private Vector3 _prevSpawnPos;

    private GameObject[] _platforms;
    private int _currentPlatformIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Get the width and height of the view area
        float distance = Vector3.Distance(this.transform.position, camera.transform.position);
        Vector3 viewBottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 viewTopRight = camera.ViewportToWorldPoint(new Vector3(1, 1, distance));
        
        // Calculate the amount of platforms to have in that area
        var platformScale = platform.transform.localScale;
        
        _areaWidth = (viewTopRight.x - viewBottomLeft.x) - platformScale.x;
        _areaHeight = viewTopRight.y - viewBottomLeft.y;
        _gridUnitHeight = platformScale.y;
        _gridHeight = (int)(_areaHeight / _gridUnitHeight);

        _numberOfPlatforms = _gridHeight;

        _platforms = new GameObject[_numberOfPlatforms];
        
        // Initialize the platform game objects
        for (int i = 0; i < _numberOfPlatforms; i++)
        {
            _platforms[i] = Instantiate(platform, new Vector3(0, 0, 0), Quaternion.identity, transform);
            _platforms[i].SetActive(false);
        }
        
        // Spawn the initial bunch
        for (float gridY = 0; gridY < _gridHeight; gridY += _gridUnitHeight)
        {
            float posY = gridY - (_gridHeight / 2.0f);
            SpawnPlatform(posY);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((_prevSpawnCheckPos.y - this.transform.position.y) >= _gridUnitHeight)
        {
            var position = this.transform.position;

            float yPos = (_gridHeight - (_gridHeight / 2.0f)) -
                         ((_prevSpawnCheckPos.y - position.y) - _gridUnitHeight);
            
            SpawnPlatform(yPos);
            _prevSpawnCheckPos = position;
        }
    }

    void SpawnPlatform(float y)
    {
        if (Random.Range(0.0f, 1.0f) <= spawnChance || (_prevSpawnPos.y - transform.position.y) > (maxPlatformDistance * _gridUnitHeight))
        {
            float posX = Random.Range(0, _areaWidth) - (_areaWidth / 2.0f);

            _platforms[_currentPlatformIdx].transform.position = new Vector3(posX, y, 0);
            _platforms[_currentPlatformIdx].SetActive(true);
            _currentPlatformIdx = (_currentPlatformIdx + 1) % _numberOfPlatforms;
            
            _prevSpawnPos = this.transform.position;
        }
    }
}
