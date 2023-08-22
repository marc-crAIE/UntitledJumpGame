using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Serialized Fields
        [SerializeField] private float moveSpeed = 5.0f;
        [SerializeField] private float jumpForce = 6.0f;
    #endregion SerializeFields
    #region Variables
        private Rigidbody _myRigidbody;
        private BoxCollider _myCollider;
        private Vector2 _inputVector;
        
        // Variables for stopping air-jumping <- No function created yet to utilise these
        private bool _groundedPlayer;
        private bool _jumpPressed = false;
        
    #endregion Variables
    
    private void Awake()
    {
        _myRigidbody = GetComponent<Rigidbody>();
        _myCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        Move();
    }

#region Move
    private void Move()
    {
        Vector3 movementDelta = _inputVector * moveSpeed * Time.deltaTime;
        transform.position += movementDelta;
    }
    private void OnMove(InputValue value)
    {
        _inputVector = value.Get<Vector2>();
    }
#endregion Move

#region Jump
    private void OnJump()
    {
        if (_groundedPlayer == true)
        {
            // Need to add a check to see if touching the layer 'ground' to stop air jumping
            _myRigidbody.velocity = new Vector2(_myRigidbody.velocity.x, jumpForce);
            _groundedPlayer = false;
        }
    }

    void OnCollisionEnter()
    {
        _groundedPlayer = true;
    }
#endregion Jump
    
}
