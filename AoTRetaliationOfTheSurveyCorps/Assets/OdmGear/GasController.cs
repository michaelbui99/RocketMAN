using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GasController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject particles;

    [Header("Settings")]
    [SerializeField]
    private float force= 1f;

    [SerializeField]
    private float maxGasCapacity = 100f;

    private Rigidbody _rigidbody;

    private bool _isActive = false;

    void Start()
    {
        _rigidbody = player.GetComponent<Rigidbody>();
        particles.SetActive(_isActive);
    }

    private void FixedUpdate()
    {
        if (_isActive)
        {
            _rigidbody.AddForce(Vector3.forward * force);
        }
    }

    void Update()
    {
        particles.SetActive(_isActive);
    }

    public void OnGas(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isActive = true;
        }

        if (context.canceled)
        {
            _isActive = false;
        }
    }
}