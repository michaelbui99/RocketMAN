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
    private EventHandler _onGrounded;

    // Start is called before the first frame update
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _onGrounded += (_, _) => _isGrounded = true;
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

    private void Update()
    {
        // NOTE: (mibui 2023-03-10) Might not be totally safe to do the detection this way, since there might be
        //                          very rare cases where the users vertical velocity is 0 while mid air when using
        //                          the OEM. Added a way to detect using events to ease migration if needed.
        _isGrounded = _rigidbody.velocity.y == 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            OnGrounded(EventArgs.Empty);
        }
    }

    private void OnGrounded(EventArgs args)
    {
        _onGrounded?.Invoke(this, args);
    }

}