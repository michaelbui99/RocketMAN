using System;
using System.Linq;
using Modules.Weapons.Common.Scripts;
using Modules.Weapons.Common.Scripts.Launchers;
using UnityEngine;

namespace Modules.Weapons.StickyBombLauncher.Scripts
{
    public class StickyBomb : MonoBehaviour, IProjectile
    {
        [field: Header("Settings")]
        [field: SerializeField]
        public float Speed { get; set; }

        [field: SerializeField] public float ExplosionForce { get; set; } = 2000f;
        [field: SerializeField] public float ExplosionRadius { get; set; } = 10f;
        [field: SerializeField] public GameObject ExplosionParticles { get; set; }

        private AudioSource _explosionSound;
        private Vector3 _stickPoint = Vector3.zero;

        private void Awake()
        {
            _explosionSound = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (_stickPoint == Vector3.zero)
            {
                return;
            }
            gameObject.transform.position = _stickPoint;
        }

        public void OnCollision(Collision collision)
        {
            _stickPoint = collision.transform.position;
        }

        public void Activate(Vector3 destination)
        {
            // No activation sequence
        }

        public void TriggerAlternateAction()
        {
            TriggerExplosion();
        }

        private void TriggerExplosion()
        {
            Explode();
            var particles = Instantiate(ExplosionParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(particles, 1);
        }

        private void Explode()
        {
            // NOTE: (mibui 2023-04-17): Reference from Rigidbody.AddExplosionForce API documentation
            Vector3 explosionPosition = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPosition, ExplosionRadius);
            _explosionSound.Play();
            colliders.ToList().ForEach(hit =>
            {
                var rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(ExplosionForce, explosionPosition, ExplosionRadius, 100.0f,
                        ForceMode.VelocityChange);
                }
            });
        }
    }
}