using System.Linq;
using Modules.Weapons.Common.Scripts;
using Unity.Mathematics;
using UnityEngine;

namespace Modules.Weapons.RocketLauncher.Scripts
{
    public class Rocket : MonoBehaviour, IProjectile
    {
        [field: Header("Settings")]
        [field: SerializeField]
        public float Speed { get; set; }

        [field: SerializeField] public float ExplosionForce { get; set; } = 2000f;
        [field: SerializeField] public float ExplosionRadius { get; set; } = 10f;
        [field: SerializeField] public GameObject ExplosionParticles { get; set; }


        private Rigidbody _rigidbody;
        private bool _active = false;
        private Vector3 _destination;
        private Vector3 _initialPosition;
        private float _distanceToTravel;
        private LineRenderer _lr;
        private AudioSource _explosionSound;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _explosionSound = GetComponent<AudioSource>();
            _explosionSound.enabled = true;
            _explosionSound.loop = false;
            _explosionSound.playOnAwake = false;
            _lr = gameObject.AddComponent<LineRenderer>();
            _lr.startWidth = 0.1f;
            _lr.endWidth = 0.1f;
        }

        private void FixedUpdate()
        {
            if (!_active)
            {
                return;
            }

            _lr.SetPosition(0, transform.position);
            _lr.SetPosition(1, _destination);
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnCollision(collision);
        }

        public void OnCollision(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // NOTE: (mibui 2023-04-18) The player shouldn't be able to rocket jump off it's own body
                return;
            }

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
                    rb.AddExplosionForce(ExplosionForce, explosionPosition, ExplosionRadius, 5.0f,
                        ForceMode.VelocityChange);
                }
            });
        }


        public void Activate(Vector3 destination)
        {
            var pos = transform.position;
            _initialPosition = pos;
            _destination = destination;
            _active = true;
            Destroy(this, 30);
        }

        public void TriggerAlternateAction()
        {
            // No alternate action
        }
    }
}