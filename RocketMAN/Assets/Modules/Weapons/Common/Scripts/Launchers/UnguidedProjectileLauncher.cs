using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.Weapons.Common.Scripts.Launchers
{
    public class UnguidedProjectileLauncher : MonoBehaviour, IProjectileLauncher
    {
        [Header("References")]
        [SerializeField]
        private GameObject launchPoint;

        [SerializeField]
        private GameObject projectilePrefab;

        private readonly List<ProjectileInstance> _projectileInstances = new();
        private UnityEngine.Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            ClearDestroyedInstances();
        }

        public void Launch()
        {
            var projectileObject = Instantiate(projectilePrefab);
            var projectile = projectileObject.GetComponent<IProjectile>();
            var projectileRb = projectileObject.GetComponent<Rigidbody>();
            projectile.Activate(Vector3.zero);

            projectileObject.transform.position = launchPoint.transform.position;
            projectileObject.transform.forward = _mainCamera.transform.forward;

            _projectileInstances.Add(new ProjectileInstance
            {
                Projectile = projectile,
                Instance = projectileObject,
                InstanceRb = projectileRb
            });
            
            projectileRb.AddRelativeForce(Vector3.forward * projectile.Speed, ForceMode.VelocityChange);
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
    }
}