using System.Linq;
using UnityEngine;

namespace Modules.Weapons.Common.Scripts.Behaviours
{
    public class ExplosionBehaviour : MonoBehaviour, IBehaviour
    {
        [field: SerializeField] public float ExplosionForce { get; set; } = 2000f;
        [field: SerializeField] public float ExplosionRadius { get; set; } = 10f;
        [field: SerializeField] public float UpwardsModifier { get; set; } = 100f;
        [field: SerializeField] public GameObject ExplosionParticles { get; set; }
        [field: SerializeField] public AudioSource ExplosionSound { get; set; }

        private void Explode()
        {
            // NOTE: (mibui 2023-04-17): Reference from Rigidbody.AddExplosionForce API documentation
            Vector3 explosionPosition = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPosition, ExplosionRadius);
            ExplosionSound.Play();
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

        public void Act()
        {
            Explode();
            var particles = Instantiate(ExplosionParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(particles, 1);
        }
    }
}