using UnityEngine;

/// <summary>
/// The script for controlling the behaviour of platforms
/// </summary>
public class PlatformScript : MonoBehaviour
{
    #region Variables

    public enum PlatformType
    {
        Basic,
        Moving,
        Breakable
    }
    
    public PlatformType type;
    
    public float areaHeightOffset = 1.0f;
    public float moveSpeed = 2.0f;

    public Color basicColor = new Color32(124, 245, 131, 255);
    public Color movingColor = new Color32(61, 161, 241, 255);
    public Color breakableColor = new Color32(147, 95, 53, 255);
    
    private Camera _camera;

    private float _areaWidth;
    private float _areaHeight;
    
    private int moveDir = 1;
    
    #endregion
    
    #region Unity Events
    
    /// <summary>
    /// The initial function called when the platform is instantiated
    /// </summary>
    void Start()
    {
        // Get the main scene camera
        _camera = Camera.main;
        
        // Get the width and height of the view area
        float distance = Vector3.Distance(this.transform.position, _camera!.transform.position);
        Vector3 viewBottomLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 viewTopRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, distance));
        
        // Get the area width and height in screen space units
        _areaWidth = (viewTopRight.x - viewBottomLeft.x) + (this.transform.localScale.x * 2.0f);
        _areaHeight = viewTopRight.y - viewBottomLeft.y + areaHeightOffset;
        
        // Make sure that platform color is set correctly based on its type
        SetColorFromType();
        
        // Pick a random move direction either left or right (for moving platforms)
        moveDir = Random.Range(1, 10) % 2 == 0 ? 1 : -1;
    }

    /// <summary>
    /// Sets the platforms type and changes its color
    /// </summary>
    /// <param name="newType">The new platform type</param>
    public void SetType(PlatformType newType)
    {
        // Set the type and ensure it is the right color
        type = newType;
        SetColorFromType();
    }

    /// <summary>
    /// Set the color of the platform based on its type
    /// </summary>
    private void SetColorFromType()
    {
        switch (type)
        {
            case PlatformType.Basic:
                gameObject.GetComponent<Renderer>().material.color = basicColor;
                break;
            case PlatformType.Moving:
                gameObject.GetComponent<Renderer>().material.color = movingColor;
                break;
            case PlatformType.Breakable:
                gameObject.GetComponent<Renderer>().material.color = breakableColor;
                break;
        }
    }

    /// <summary>
    /// Update the platform once per frame
    /// </summary>
    void Update()
    {
        // If the y position is below the view area, deactivate the platform
        if (transform.position.y < -(_areaHeight / 2.0f))
            gameObject.SetActive(false);
        
        // If the platform type is a moving platform, handle movement
        if (type == PlatformType.Moving)
        {
            // Get the platform size and half of the areas width
            float platformSize = transform.localScale.x;
            float areaHalfWidth = (_areaWidth / 2.0f) - platformSize;
            
            // If the platform is too far to the left or to the right, flip the direction
            if ((transform.position.x - platformSize <= -areaHalfWidth && moveDir == -1) || 
                (transform.position.x + platformSize >= areaHalfWidth && moveDir == 1))
                moveDir *= -1;
            
            // Move the platform based on the direction and movement speed
            transform.position += Vector3.right * (moveDir * moveSpeed * Time.deltaTime); 
        }
    }

    /// <summary>
    /// Called when the platform is bounced on by the player
    /// </summary>
    public void OnBouncedFrom()
    {
        // If the platform is breakable, break it
        if (type == PlatformType.Breakable)
            gameObject.SetActive(false);
    }
    
    #endregion
}