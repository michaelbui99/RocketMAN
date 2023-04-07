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
            }

            if (!context.canceled)
            {
                return;
            }

            _isActive = false;
            particles.Stop();
        }

        public void SpendGas(float amount)
        {
            if (_currentGasLevel <= 0)
            {
                return;
            }

            bool amountExceedsCurrentGasLevel = (_currentGasLevel - amount) < 0;
            if (amountExceedsCurrentGasLevel)
            {
                _currentGasLevel = 0;
                return;
            }

            _currentGasLevel -= amount;
        }

        public bool GasTankEmpty()
        {
            return _currentGasLevel <= 0;
        }
    }
}