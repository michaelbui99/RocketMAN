using System;
using System.Linq;
using UnityEngine;
using Weapons.Common.Scripts;

namespace Weapons.RocketLauncher.Scripts
{
    public class Rocket : MonoBehaviour, IProjectile
    {
        [field: Header("Settings")]
        [field: SerializeField]
        public float Speed { get; set; }

        [field: SerializeField] public float ExplosionForce { get; set; } = 2000f;
        [field: SerializeField] public float ExplosionRadius { get; set; } = 10f;


        private Rigidbody _rigidbody;
        private bool _active = false;
        private Vector3 _direction;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (!_active)
            {
                return;
            }

            _rigidbody.AddForce(_direction * (Speed * Time.fixedDeltaTime), ForceMode.Acceleration);
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnCollision(collision);
        }

        public void OnCollision(Collision collision)
        {
            // NOTE: (mibui 2023-04-17): Reference from Rigidbody.AddExplosionForce API documentation
            Vector3 explosionPosition = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPosition, ExplosionRadius);
            
            colliders.ToList().ForEach(hit =>
            {
                var rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(ExplosionForce, explosionPosition, ExplosionRadius, 3.0f, ForceMode.VelocityChange);
                }
            });
            
            Destroy(gameObject);
        }

        public void Activate(Vector3 direction)
        {
            _direction = direction;
            _active = true;
        }
    }
}