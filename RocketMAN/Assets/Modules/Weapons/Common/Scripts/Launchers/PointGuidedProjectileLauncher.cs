using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

namespace Modules.Weapons.Common.Scripts.Launchers
{
    public class PointGuidedProjectileLauncher : MonoBehaviour, IProjectileLauncher
    {
        [Header("References")]
        [SerializeField]
        private GameObject launchPoint;

        [SerializeField]
        private GameObject projectilePrefab;

        [Header("Settings")]
        [SerializeField]
        private LayerMask validDestinationLayers;

        private readonly List<ProjectileInstance> _projectileInstances = new();

        private UnityEngine.Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = UnityEngine.Camera.main;
        }


        public void FixedUpdate()
        {
            ClearDestroyedInstances();
            _projectileInstances.ForEach(i =>
            {
                MoveProjectileTowardsDestination(i.InstanceRb, i.Projectile, i.Destination);
            });
        }

        public void Launch()
        {
            Ray ray = RayCastUtil.GetRayToCenterOfScreen(_mainCamera);
            var destination = Physics.Raycast(ray, out RaycastHit hit, 1000f, validDestinationLayers)
                ? hit.point
                : ray.GetPoint(1000f);
            
            var projectileObject = Instantiate(projectilePrefab);
            var projectile = projectileObject.GetComponent<IProjectile>();
            var projectileRb = projectileObject.GetComponent<Rigidbody>();
            projectile.Activate(destination);
            projectileObject.transform.position = launchPoint.transform.position;
            projectileObject.transform.forward = _mainCamera.transform.forward;
            
            _projectileInstances.Add(new ProjectileInstance
            {
                Destination = destination,
                Instance = projectileObject,
                InstanceRb = projectileRb,
                Projectile = projectile
            });
        }

        public List<IProjectile> GetActiveProjectiles()
        {
            return _projectileInstances
                .Select(instance => instance.Projectile)
                .ToList();
        }

        private void ClearDestroyedInstances()
        {
            _projectileInstances.ToList().ForEach(i =>
            {
                if (i.Instance != null)
                {
                    return;
                }

                _projectileInstances.Remove(i);
            });
        }

        private void MoveProjectileTowardsDestination(Rigidbody rb, IProjectile projectile, Vector3 destination)
        {
            var direction = (destination - rb.transform.position).normalized;
            rb.gameObject.transform.forward = direction;

            rb.AddForce(direction * (projectile.Speed * Time.fixedDeltaTime), ForceMode.Acceleration);
        }
    }
}