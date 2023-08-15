using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public float areaHeightOffset = 1.0f;
    
    private Camera _camera;

    private float _areaWidth;
    private float _areaHeight;
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        
        // Get the width and height of the view area
        float distance = Vector3.Distance(this.transform.position, _camera.transform.position);
        Vector3 viewBottomLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 viewTopRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, distance));
        
        _areaWidth = (viewTopRight.x - viewBottomLeft.x) + (this.transform.localScale.x * 2.0f);
        _areaHeight = viewTopRight.y - viewBottomLeft.y + areaHeightOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -(_areaHeight / 2.0f))
            gameObject.SetActive(false);
    }
}
