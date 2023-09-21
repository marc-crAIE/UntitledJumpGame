using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    public bool alive = true;
    private Transform _transform;
    private Rigidbody rb;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float fallMultiplier;
    private Vector3 vecGravity;

    public PlatformSpawnerScript platformMover;
    
    // Start is called before the first frame update
    void Start()
    {
        vecGravity = new Vector3(0, -Physics.gravity.y, 0f);
        rb = GetComponent<Rigidbody>();
        _transform = transform;
    }

    void FixedUpdate()
    {
        var velocity = vecGravity * (fallMultiplier * Time.deltaTime);
        
        if (rb.velocity.y < 0f)
        {
            rb.velocity -= velocity;
        }
        if (this.rb.position.y >= -transform.localScale.y)
        {
            if (platformMover)
            {
                platformMover.MovePlatforms(rb.velocity.y);
                this.rb.position = new Vector3(rb.position.x, -transform.localScale.y, rb.position.z);
            }
        }
    }
    
    void OnTriggerEnter(Collider collision)
    {
        if ((collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Spawn Platform")) && rb.velocity.y <= 0 && alive)
        {
            // Get the platform's script and call its bounced from event
            var platformScript = collision.gameObject.GetComponent<PlatformScript>();
            if (platformScript)
                platformScript.OnBouncedFrom();
                
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);            
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            alive = false;
            rb.velocity = Vector3.zero;
            fallMultiplier = 10f;
        }
    }
}