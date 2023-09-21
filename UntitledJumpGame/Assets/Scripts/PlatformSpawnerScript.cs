using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// The Platform Spawner is responsible for managing the spawning of platforms and essentially
/// the core of the level design
/// </summary>
public class PlatformSpawnerScript : MonoBehaviour
{
    #region Variables

    #region Public

    public GameObject platform;
    public float spawnChance;
    // The max number of platform spaces between each spawned platform
    public int maxPlatformSkips;
    public int spawnPlatformStartHeight = 8;
    public int spaceAfterSpawnPlatform = 6;
    public int maxPlatformXSpacing = 4;

    public float basicPlatformChance = 0.85f;
    public float movingPlatformChance = 0.1f;
    public float breakablePlatformChance = 0.05f;
        
    #endregion

    #region Private

    private Camera _camera;

    private float _areaWidth;
    private float _areaHeight;
    private int _gridHeight;
    private float _gridUnitHeight;
    private int _numberOfPlatforms;

    private Vector3 _prevSpawnPos;
    private Vector3 _prevSpawnCheckPos;
    private int _spawnSkipCount;

    private GameObject[] _platforms;
    private int _currentPlatformIdx;

    #endregion

    #endregion

    #region Unity Events

    /// <summary>
    /// The initial function called when the platform spawner is instantiated
    /// </summary>
    void Start()
    {
        // Get the main camera
        _camera = Camera.main;
        
        // Get the width and height of the view area along with the size of the grid of platforms
        float distance = Vector3.Distance(this.transform.position, _camera!.transform.position);
        Vector3 viewBottomLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 viewTopRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, distance));
        
        // Get the scale of the platform object
        var platformScale = platform.transform.localScale;
        
        // Get the area width and height along with the platform vertical grid heights
        viewBottomLeft -= platformScale;
        viewTopRight -= platformScale;
        
        _areaWidth = (viewTopRight.x - viewBottomLeft.x) - platformScale.x;
        _areaHeight = viewTopRight.y - viewBottomLeft.y;
        _gridUnitHeight = platformScale.y;
        _gridHeight = (int)(_areaHeight * 1.5f);

        // Calculate the amount of platforms to have in the area
        _numberOfPlatforms = _gridHeight;
        
        // Setup the platforms array
        _platforms = new GameObject[_numberOfPlatforms];
        
        // Initialize the platform game objects
        for (int i = 0; i < _numberOfPlatforms; i++)
        {
            // Create a new platform game object and deactivate it
            _platforms[i] = Instantiate(platform, new Vector3(0, 0, 0), Quaternion.identity, transform);
            _platforms[i].SetActive(false);
        }
        
        // Spawn the initial bunch
        int count = 0;
        for (float gridY = 0; gridY < _gridHeight; gridY += _gridUnitHeight)
        {
            if (count <= spawnPlatformStartHeight || count > spawnPlatformStartHeight + spaceAfterSpawnPlatform)
            {
                // Get the y position factoring the offset of half the grid height
                float posY = gridY - (_gridHeight / 2.0f);
                
                // CHeck if the platform should be the spawn platform
                if (count == spawnPlatformStartHeight)
                    SpawnPlatform(posY, 0, true);
                // Spawn a regular platform
                else
                    SpawnPlatform(posY);
            }

            count++;
        }
    }

    /// <summary>
    /// Update the platform spawner once per frame
    /// </summary>
    void Update()
    {
        // Check if the world has moved 1 grid unit higher
        if ((_prevSpawnCheckPos.y - this.transform.position.y) >= _gridUnitHeight)
        {
            var position = this.transform.position;
            
            // Calculate the y position of the next platform
            float yPos = (_gridHeight - (_gridHeight / 2.0f)) -
                         ((_prevSpawnCheckPos.y - position.y) - _gridUnitHeight);
            
            // Spawn a platform at the calculated y position
            SpawnPlatform(yPos);
            _prevSpawnCheckPos = position;
        }
        
        // TEMPORARY: Used to test platform generation without the player controller
        //transform.position -= new Vector3(0, 2.0f, 0.0f) * Time.deltaTime;
    }

    #endregion
    
    /// <summary>
    /// Spawn a new platform at a given Y position
    /// </summary>
    /// <param name="y">The Y position to spawn the platform at</param>
    /// <param name="x">An optional X position to force the platform to spawn at if the spawn variable is true</param>
    /// <param name="spawn">Is the platform the spawn platform</param>
    void SpawnPlatform(float y, float x = 0, bool spawn = false)
    {
        // Randomly pick if a platform should spawn unless it has been too long since one has spawned
        if (Random.Range(0.0f, 1.0f) <= spawnChance || _spawnSkipCount >= maxPlatformSkips || spawn)
        {
            // Pick a random x position
            float areaHalfWidth = _areaWidth / 2.0f;
            float xMin = _prevSpawnPos.x - maxPlatformXSpacing <= -areaHalfWidth
                ? -areaHalfWidth
                : _prevSpawnPos.x - maxPlatformXSpacing;
            float xMax = _prevSpawnPos.x + maxPlatformXSpacing >= areaHalfWidth
                ? areaHalfWidth
                : _prevSpawnPos.x + maxPlatformXSpacing;
            float posX = !spawn ? Random.Range(xMin, xMax) : x;
            var spawnPos = new Vector3(posX, y, 0);
            
            // Set the transform position of the platform and re-activate it
            _platforms[_currentPlatformIdx].transform.position = spawnPos;
            _platforms[_currentPlatformIdx].SetActive(true);
            
            // Set the type (does not effect the spawn platform)
            // TODO: This code is disgusting and very temporary. Fix it up later
            float typeChance = Random.Range(0.0f, 1.0f);
            if (typeChance <= basicPlatformChance || spawn)
                _platforms[_currentPlatformIdx].GetComponent<PlatformScript>().SetType(PlatformScript.PlatformType.Basic);
            else if (typeChance <= basicPlatformChance + movingPlatformChance)
                _platforms[_currentPlatformIdx].GetComponent<PlatformScript>().SetType(PlatformScript.PlatformType.Moving);
            else
                _platforms[_currentPlatformIdx].GetComponent<PlatformScript>().SetType(PlatformScript.PlatformType.Breakable);
            
            // Set the tag
            if (spawn)
                _platforms[_currentPlatformIdx].tag = "Spawn Platform";
            else
                _platforms[_currentPlatformIdx].tag = "Platform";
            
            // Increment the current platform index between 0 and the number of platforms
            _currentPlatformIdx = (_currentPlatformIdx + 1) % _numberOfPlatforms;
            // Reset the spawn skip count
            _spawnSkipCount = 0;

            _prevSpawnPos = _platforms[_currentPlatformIdx].transform.position;
        }
        else
        {
            // We have skipped a platform spawn.. increment the skip count
            _spawnSkipCount++;
        }
    }
    
    /// <summary>
    /// Move the platforms by a given distance
    /// </summary>
    /// <param name="moveDistance">The distance to move the platforms by</param>
    public void MovePlatforms(float moveDistance)
    {
        transform.position -= new Vector3(0, moveDistance * Time.deltaTime, 0);
        ScoreManager._instance.AddScore(moveDistance * 0.1f);
    }
}
