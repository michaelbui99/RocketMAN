using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Modules.Odm.Scripts
{
    public class GasController : MonoBehaviour, IGasController
    {
        [Header("References")]
        [SerializeField]
        private Rigidbody ownerRigidbody;

        [Header("Settings")]
        [SerializeField]
        private float propulsionSpeed;

        [SerializeField]
        private float maxGasCapacity;

        [SerializeField]
        private float gasExpenditureRatePerSecond;
        
        private IOdmInput _input;
        private bool _active = false;
        private float _currentGasLevel;

        public event IGasController.SimpleGasEvent OnGasActivated;
        public event IGasController.SimpleGasEvent OnGasReleased;

        private void Awake()
        {
            _input = GetComponent<IOdmInput>();
            _input.OnGasActivate += ActivateGas;
            _input.OnGasRelease += ReleaseGas;
        }

        private void OnDestroy()
        {
            _input.OnGasActivate -= ActivateGas;
            _input.OnGasRelease -= ReleaseGas;
        }

        private void Start()
        {
            _currentGasLevel = maxGasCapacity;
        }

        private void FixedUpdate()
        {
            if (!_active || GasTankEmpty())
            {
                return;
            }
            
            ExpendGas(gasExpenditureRatePerSecond * Time.fixedDeltaTime);
            PropelOwnerForward();
        }

        private void ActivateGas()
        {
            if (GasTankEmpty())
            {
                return;
            }
            
            _active = true;
            OnGasActivated?.Invoke();
        }

        private void ReleaseGas()
        {
            _active = false;
            OnGasReleased?.Invoke();
        }

        private bool GasTankEmpty() => _currentGasLevel <= 0;

        private void ExpendGas(float amount)
        {
            if (amount > _currentGasLevel)
            {
                _currentGasLevel = 0;
                return;
            }

            _currentGasLevel -= amount;
        }

        private void RefillGasTank()
        {
            _currentGasLevel = maxGasCapacity;
        }

        private void PropelOwnerForward()
        {
            var direction = ownerRigidbody.transform.forward.normalized;
            ownerRigidbody.AddForce(direction * (propulsionSpeed * Time.fixedDeltaTime), ForceMode.VelocityChange);
        }
    }
}