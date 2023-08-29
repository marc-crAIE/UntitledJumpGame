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
    #endregion SerializeFields
    #region Variables
        private Rigidbody rb;
        private Vector2 inputVector;
    #endregion Variables
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
    }
 #region Move
    private void Move()
    {
        Vector3 movementDelta = new Vector3(inputVector.x * moveSpeed * Time.deltaTime, inputVector.y * Time.deltaTime, 0f);
        transform.position += movementDelta;
    }
    private void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }
#endregion Move
}
