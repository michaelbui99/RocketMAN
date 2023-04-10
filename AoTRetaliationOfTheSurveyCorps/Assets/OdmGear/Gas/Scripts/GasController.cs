using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OdmGear.Gas
{
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
        private AudioSource _gasAudio;

        public float GetCurrentGasLevel() => _currentGasLevel;
        public float GetMaxGasCapacity() => maxGasCapacity;

        private void Awake()
        {
            _rigidbody = player.GetComponent<Rigidbody>();
            _gasAudio = GetComponent<AudioSource>();
        }

        void Start()
        {
            _currentGasLevel = maxGasCapacity;
            particles.Stop();
        }

        private void FixedUpdate()
        {
            if (!_isActive || GasTankEmpty())
            {
                return;
            }

            _rigidbody.AddForce(player.transform.forward * force);
        }

        void Update()
        {
            if (_isActive)
            {
                SpendGas(depletionRate * Time.deltaTime);
            }
        }

        public void OnGas(InputAction.CallbackContext context)
        {
            if (context.started && _currentGasLevel >= 0)
            {
                _isActive = true;
                particles.Play();
                Debug.Log("Playing audio");
                _gasAudio.Play();
            }

            if (!context.canceled)
            {
                return;
            }

            _isActive = false;
            _gasAudio.Stop();
            particles.Stop();
        }

        public bool SpendGas(float amount)
        {
            if (_currentGasLevel <= 0)
            {
                return false;
            }

            if (amount > _currentGasLevel)
            {
                _currentGasLevel = 0;
            }

            _currentGasLevel -= amount;
            return true;
        }

        public bool GasTankEmpty()
        {
            return _currentGasLevel <= 0;
        }

        public bool HasEnoughGas(float amount)
        {
            return _currentGasLevel >= amount;
        }
    }
}