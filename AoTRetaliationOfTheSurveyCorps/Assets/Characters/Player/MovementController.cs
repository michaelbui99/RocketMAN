using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.VFX;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerInput playerInput;

    [Header("Speed")]
    [SerializeField]
    private float verticalSpeed = 20f;

    [SerializeField]
    private float horizontalSpeed = 500f;

    private Rigidbody _rigidbody;
    private Vector3 _horizontalMovementVector;
    private EventHandler<bool> _onGroundedChange;
    private bool _isGrounded = false;

    // Start is called before the first frame update
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _onGroundedChange += (_, isGrounded) => _isGrounded = isGrounded;
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
        if (_isGrounded)
        {
            _rigidbody.AddRelativeForce(_horizontalMovementVector);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            OnGroundedChange(true);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            OnGroundedChange(false);
        }
    }

    private void OnGroundedChange(bool isGrounded)
    {
        _onGroundedChange?.Invoke(this, isGrounded);
    }
}