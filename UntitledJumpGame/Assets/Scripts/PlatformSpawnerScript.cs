using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlatformSpawnerScript : MonoBehaviour
{
    public GameObject platform;
    public float spawnChance;
    // The max number of platform spaces between each spawned platform
    public int maxPlatformSkips;
    
    private Camera _camera;

    private float _areaWidth;
    private float _areaHeight;
    private int _gridHeight;
    private float _gridUnitHeight;
    private int _numberOfPlatforms;

    private Vector3 _prevSpawnCheckPos;
    private int _spawnSkipCount = 0;

    private GameObject[] _platforms;
    private int _currentPlatformIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        
        // Get the width and height of the view area
        float distance = Vector3.Distance(this.transform.position, _camera.transform.position);
        Vector3 viewBottomLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 viewTopRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, distance));
        
        // Calculate the amount of platforms to have in that area
        var platformScale = platform.transform.localScale;
        
        _areaWidth = (viewTopRight.x - viewBottomLeft.x) - platformScale.x;
        _areaHeight = viewTopRight.y - viewBottomLeft.y;
        _gridUnitHeight = platformScale.y;
        _gridHeight = (int)(_areaHeight * 1.5f);

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
        
        // TEMPORARY
        transform.position -= new Vector3(0, 2.0f, 0.0f) * Time.deltaTime;
    }

    void SpawnPlatform(float y)
    {
        if (Random.Range(0.0f, 1.0f) <= spawnChance || _spawnSkipCount >= maxPlatformSkips)
        {
            float posX = Random.Range(0, _areaWidth) - (_areaWidth / 2.0f);
            var spawnPos = new Vector3(posX, y, 0);

            _platforms[_currentPlatformIdx].transform.position = spawnPos;
            _platforms[_currentPlatformIdx].SetActive(true);
            _currentPlatformIdx = (_currentPlatformIdx + 1) % _numberOfPlatforms;
            _spawnSkipCount = 0;
        }
        else
        {
            _spawnSkipCount++;
        }
    }
}
