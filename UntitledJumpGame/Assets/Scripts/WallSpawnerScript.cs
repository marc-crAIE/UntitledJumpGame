using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WallSpawnerScript : MonoBehaviour
{
    public GameObject wall;
    
    private Camera _camera;
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        
        // Get the width and height of the view area
        float distance = Vector3.Distance(this.transform.position, _camera.transform.position);
        Vector3 viewBottomLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 viewTopRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, distance));
        
        float areaWidth = (viewTopRight.x - viewBottomLeft.x) + (wall.transform.localScale.x * 2.0f);
        float areaHeight = viewTopRight.y - viewBottomLeft.y;
        
        // Get the wall x position
        float xPos = areaWidth / 2.0f;
        
        // Create the wall at the appropriate x position given the side it spawns
        var wallRight = Instantiate(wall, new Vector3(xPos, 0, 0), Quaternion.identity, transform);
        var wallLeft = Instantiate(wall, new Vector3(-xPos, 0, 0), Quaternion.identity, transform);
        
        // Set the y scale of the wall to be just over the height of the screen
        var wallTransform = wall.transform;
        var wallLocalScale = wallTransform.localScale;
        var wallScale = new Vector3(wallLocalScale.x, areaHeight * 1.5f, wallLocalScale.z);
        
        wallRight.transform.localScale = wallScale;
        wallLeft.transform.localScale = wallScale;
    }
}
