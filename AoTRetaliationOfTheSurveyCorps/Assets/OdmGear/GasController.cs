using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject player;
    
    private Rigidbody _rigidbody;

    private bool _isActive = false;
    
    void Start()
    {
        _rigidbody = player.GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        
    }
    
}
