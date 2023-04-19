using System.Linq;
using Modules.Weapons.Common.Scripts;
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


        private Rigidbody _rigidbody;
        private bool _active = false;
        private Vector3 _destination;
        private Vector3 _initialPosition;
        private float _distanceToTravel;
        private LineRenderer _lr;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
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
            MoveRocketTowardsDestination();
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
            Destroy(gameObject);
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
                    rb.AddExplosionForce(ExplosionForce, explosionPosition, ExplosionRadius, 3.0f,
                        ForceMode.VelocityChange);
                }
            });
        }

        private float GetDistanceToTravel()
        {
            return Vector3.Distance(_destination, _initialPosition);
        }

        private float GetDistanceTraveled()
        {
            return Vector3.Distance(transform.position, _initialPosition);
        }

        private void MoveRocketTowardsDestination()
        {
            var direction = (_destination - transform.position).normalized;
            gameObject.transform.forward = direction;

            _rigidbody.AddForce(direction * (Speed * Time.fixedDeltaTime), ForceMode.Acceleration);
        }

        private void MoveRocketTowardsDestination(float forceScalar)
        {
            var direction = (_destination - transform.position).normalized;
            gameObject.transform.forward = direction;

            _rigidbody.AddForce(direction * (Speed * forceScalar * Time.fixedDeltaTime), ForceMode.Acceleration);
        }

        private void InitialRocketPropulsion()
        {
            MoveRocketTowardsDestination(20);
        }

        public void Activate(Vector3 destination)
        {
            var pos = transform.position;
            _initialPosition = pos;
            _destination = destination;
            _active = true;
            InitialRocketPropulsion();
            Destroy(this, 30);
        }
    }
}