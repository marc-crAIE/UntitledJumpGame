using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Serialized Fields
        [SerializeField] private float moveSpeed = 5.0f;
        [SerializeField] private float tiltMoveSpeed = 10.0f;
    #endregion SerializeFields
    #region Variables
        Rigidbody rb;
        Vector2 inputVector;
        private float directionX;
    #endregion Variables
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Tilt();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(directionX, rb.velocity.y);
    }

    #region Move
    private void Move()
    {
        Vector3 movementDelta = new Vector3(inputVector.x * moveSpeed * Time.deltaTime, inputVector.y * Time.deltaTime, 0f);
        transform.position += movementDelta;
    }

    private void Tilt()
    {
        directionX = Input.acceleration.x * tiltMoveSpeed;
        
        var position = transform.position;
        position = new Vector2(Mathf.Clamp(position.x, -25f, 25f), position.y);
        
        transform.position = position;
    }
    
    private void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }
#endregion Move
}
