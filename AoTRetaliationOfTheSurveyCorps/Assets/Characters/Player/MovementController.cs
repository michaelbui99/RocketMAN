using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private GameObject cinemachine;

    [Header("Speed")]
    [SerializeField]
    private float verticalSpeed = 20f;
    [SerializeField]
    private float horizontalSpeed = 500f;
    [SerializeField]
    private float rotationSpeed = 1f;

    private Rigidbody _rigidbody;
    private CinemachineBrain _cinemachineBrain;
    private Vector3 _horizontalMovementVector;
    private EventHandler _onGrounded;
    private bool _isGrounded = false;

    // Start is called before the first frame update
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _cinemachineBrain = cinemachine.GetComponent<CinemachineBrain>();
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
        gameObject.transform.forward = GetCameraForwardDirection();
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

    private Vector3 GetCameraForwardDirection()
    {
        if (Camera.main is null)
        {
            throw new NotSupportedException("No camera");
        }
        
        Vector3 cameraDirection = Camera.main.transform.forward;
        return Vector3.ProjectOnPlane(cameraDirection, Vector3.up);
    }
}