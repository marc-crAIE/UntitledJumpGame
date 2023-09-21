using UnityEngine;

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
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        
        // Get the width and height of the view area
        float distance = Vector3.Distance(this.transform.position, _camera!.transform.position);
        Vector3 viewBottomLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 viewTopRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, distance));
        
        _areaWidth = (viewTopRight.x - viewBottomLeft.x) + (this.transform.localScale.x * 2.0f);
        _areaHeight = viewTopRight.y - viewBottomLeft.y + areaHeightOffset;

        SetColorFromType();

        moveDir = Random.Range(1, 10) % 2 == 0 ? 1 : -1;
    }

    public void SetType(PlatformType newType)
    {
        type = newType;
        SetColorFromType();
    }

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

    // Update is called once per frame
    void Update()
    {
        // If the y position is below the view area, deactivate the platform
        if (transform.position.y < -(_areaHeight / 2.0f))
            gameObject.SetActive(false);

        if (type == PlatformType.Moving)
        {
            float platformSize = transform.localScale.x;
            float areaHalfWidth = (_areaWidth / 2.0f) - platformSize;
            if ((transform.position.x - platformSize <= -areaHalfWidth && moveDir == -1) || 
                (transform.position.x + platformSize >= areaHalfWidth && moveDir == 1))
                moveDir *= -1;
            
            transform.position += Vector3.right * (moveDir * moveSpeed * Time.deltaTime); 
        }
    }

    public void OnBouncedFrom()
    {
        if (type == PlatformType.Breakable)
            gameObject.SetActive(false);
    }
    
    #endregion
}