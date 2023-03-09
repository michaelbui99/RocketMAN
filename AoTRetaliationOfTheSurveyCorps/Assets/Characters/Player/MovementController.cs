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

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        _rigidbody.AddForce(new Vector3(inputMovement.x, 0, inputMovement.y) * (horizontalSpeed * Time.deltaTime));
    }

    
    // Update is called once per frame
    void Update()
    {
    }
}