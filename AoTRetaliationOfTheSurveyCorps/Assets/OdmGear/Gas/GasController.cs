using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GasController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private ParticleSystem particles;

    [Header("Settings")]
    [SerializeField]
    private float force = 1f;

    [SerializeField]
    private float maxGasCapacity = 100f;

    [SerializeField]
    private float depletionRate = 1f;

    private Rigidbody _rigidbody;

    private bool _isActive = false;
    private float _currentGasLevel;

    public float GetCurrentGasLevel() => _currentGasLevel;
    public float GetMaxGasCapacity() => maxGasCapacity;

    void Start()
    {
        _rigidbody = player.GetComponent<Rigidbody>();
        _currentGasLevel = maxGasCapacity;
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
        if (_isActive && _currentGasLevel >= 0)
        {
            _currentGasLevel -= depletionRate * Time.deltaTime;
        }
    }

    public void OnGas(InputAction.CallbackContext context)
    {
        if (context.started && _currentGasLevel >= 0)
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