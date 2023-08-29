using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float fallMultiplier;
    private Vector3 vecGravity;
    
    // Start is called before the first frame update
    void Start()
    {
        vecGravity = new Vector3(0, -Physics.gravity.y, 0f);
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb.velocity.y < 0f)
        {
            rb.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
        }
    }
    
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);            
        }
    }
}
