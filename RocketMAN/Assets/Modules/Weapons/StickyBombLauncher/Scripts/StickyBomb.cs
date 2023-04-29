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

        private bool hasStickPoint = false;

        private AudioSource _explosionSound;
        private Vector3 _stickPoint = Vector3.zero;
        private Collider _stickCollider;

        private void Awake()
        {
            _explosionSound = GetComponent<AudioSource>();
            _explosionSound.enabled = true;
            _explosionSound.loop = false;
            _explosionSound.playOnAwake = false;
        }

        private void Update()
        {
            if (hasStickPoint)
            {
                gameObject.transform.position = _stickPoint;
                Debug.Log(LayerMask.LayerToName(_stickCollider.gameObject.layer));
                return;
            }

            Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, 0.2f);
            if (!colliders.Any())
            {
                return;
            }

            var stickCandidate = colliders.First();
            if (stickCandidate.gameObject.layer == LayerMask.NameToLayer("Weapon"))
            {
                return;
            }
            
            _stickPoint = stickCandidate.ClosestPoint(gameObject.transform.position);
            _stickCollider = stickCandidate;
            hasStickPoint = true;
        }

        public void OnCollision(Collision collision)
        {
            _stickPoint = collision.transform.position;
        }

        public void Activate(Vector3 destination)
        {
            hasStickPoint = false;
        }

        public void TriggerAlternateAction()
        {
            TriggerExplosion();
        }

        private void TriggerExplosion()
        {
            _explosionSound.Play();
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
            colliders.ToList().ForEach(hit =>
            {
                var rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(ExplosionForce, explosionPosition, ExplosionRadius, 1f,
                        ForceMode.VelocityChange);
                }
            });
        }
    }
}