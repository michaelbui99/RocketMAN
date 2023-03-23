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
    private ParticleSystem particles;

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
        particles.Stop();
    }

    private void FixedUpdate()
    {
        if (_isActive)
        {
            _rigidbody.AddForce(player.transform.forward * force);
        }
    }

    void Update()
    {
    }

    public void OnGas(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isActive = true;
            particles.Play();
        }

        if (context.canceled)
        {
            _isActive = false;
            particles.Stop();
        }
    }
}