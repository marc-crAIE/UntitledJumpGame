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
        /// <summary>
        /// Player movement speed
        /// </summary>
        [SerializeField] private float moveSpeed = 5.0f;
        
        /// <summary>
        /// Movement speed for android gyro tilting
        /// </summary>
        [SerializeField] private float tiltMoveSpeed = 10.0f;

        [SerializeField] private DeathMenu deathScreen;
    #endregion SerializeFields
    #region Variables
        Rigidbody rb;
        Vector2 inputVector;
        private float directionX;
        public JumpController jumper;
    #endregion Variables
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Tilt();

        //used the script from enemy manager as it will sill provide the correct information
        if (transform.position.y < -EnemyManager._instance.GetGameHeight() * 0.6)
        {
            //set this gameObject to false
            this.transform.parent.gameObject.SetActive(false);
            ScoreManager._instance.CheckScore();
            deathScreen.InitialiseMenu();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(directionX, rb.velocity.y);
    }

    #region Move
    /// <summary>
    /// Calculates movement for player character as long as the character is alive.
    /// </summary>
    private void Move()
    {
        Vector3 movementDelta = new Vector3(inputVector.x * moveSpeed * Time.deltaTime, 0f, 0f);
        if (jumper.alive)
        {
            rb.position += movementDelta;
        }
    }

    /// <summary>
    /// Calculates movement when tilting a device
    /// </summary>
    private void Tilt()
    {
        directionX = Input.acceleration.x * tiltMoveSpeed;
        
        var position = rb.position;
        position = new Vector2(Mathf.Clamp(position.x, -25f, 25f), position.y);
        
        rb.position = position;
    }
    
    private void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }
#endregion Move
}
