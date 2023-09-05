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

    void Update()
    {
        var velocity = vecGravity * (fallMultiplier * Time.deltaTime);
        
        if (rb.velocity.y < 0f)
        {
            rb.velocity -= velocity;
        }
        if (this.transform.position.y > -transform.localScale.y)
        {
            var position = this.transform.position;
            platformMover.MovePlatforms(velocity.y * 2);
            this.transform.position = new Vector3(position.x, position.y - velocity.y, position.z);
        }
    }
    
    void OnTriggerEnter(Collider collision)
    {
        if ((collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Spawn Platform")) && rb.velocity.y <= 0 && alive)
        {
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