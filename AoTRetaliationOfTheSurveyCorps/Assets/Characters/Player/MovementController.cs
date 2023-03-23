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

    [SerializeField]
    private float verticalRotationSpeed = 1f;
    
    [SerializeField]
    private float horizontalRotationSpeed = 1f;

    private Rigidbody _rigidbody;
    private Vector3 _horizontalMovementVector;
    private EventHandler _onGrounded;
    private bool _isGrounded = false;
    private float lookX;
    private float lookY;

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

    public void OnXRotation(InputAction.CallbackContext value)
    {
        var xRotation = value.ReadValue<float>();
        lookX = xRotation * Time.deltaTime * horizontalRotationSpeed;
    }

    public void OnYRotation(InputAction.CallbackContext value)
    {
        var yRotation = value.ReadValue<float>();
        lookX = yRotation * Time.deltaTime * verticalRotationSpeed;
    }
    
    private void FixedUpdate()
    {
        _rigidbody.AddRelativeForce(_horizontalMovementVector);
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