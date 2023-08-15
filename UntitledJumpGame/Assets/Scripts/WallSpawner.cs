using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WallSpawner : MonoBehaviour
{
    public Camera camera;
    public GameObject wall;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get the width of the view area
        float distance = Vector3.Distance(this.transform.position, camera.transform.position);
        Vector3 viewBottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 viewTopRight = camera.ViewportToWorldPoint(new Vector3(1, 1, distance));
        
        float areaWidth = (viewTopRight.x - viewBottomLeft.x) + (wall.transform.localScale.x * 2.0f);
        float xPos = areaWidth / 2.0f;
        
        Instantiate(wall, new Vector3(xPos, 0, 0), Quaternion.identity, transform);
        Instantiate(wall, new Vector3(-xPos, 0, 0), Quaternion.identity, transform);
    }
}
