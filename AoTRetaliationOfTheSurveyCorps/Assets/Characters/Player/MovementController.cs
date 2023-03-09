using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [SerializeField]
    private float verticalSpeed = 20f;

    [SerializeField]
    private float horizontalSpeed = 500f;

    [SerializeField]
    private PlayerInput playerInput;

    private Rigidbody _rigidbody;
    private Vector3 _horizontalMovementVector;
    private bool _isGrounded = false;

    // Start is called before the first frame update
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (_isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * verticalSpeed);
        }
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        _horizontalMovementVector = new Vector3(inputMovement.x, 0, inputMovement.y) * horizontalSpeed;
    }

    private void FixedUpdate()
    {
        _rigidbody.AddRelativeForce(_horizontalMovementVector);
    }

    // Update is called once per frame
    void Update()
    {
        _isGrounded = _rigidbody.velocity.y == 0;
    }
}