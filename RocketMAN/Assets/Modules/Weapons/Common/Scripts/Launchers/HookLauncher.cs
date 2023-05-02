using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Weapons.Common.Scripts.Weapon;
using UnityEngine;
using Utility;

namespace Modules.Weapons.Common.Scripts.Launchers
{
    public class HookLauncher : MonoBehaviour, IProjectileLauncher
    {
        [Header("References")]
        [SerializeField]
        private GameObject launchPoint;

        [SerializeField]
        private GameObject projectilePrefab;

        [Header("Settings")]
        [SerializeField]
        private LayerMask validDestinationLayers;

        [Header("Settings")]
        [SerializeField]
        private bool laser = false;

        [SerializeField]
        private float maxDistance = 1000f;

        private LineRenderer _lineRenderer;
        private Camera _mainCamera;
        private readonly List<ProjectileInstance> _activeHooks = new();

        private void Awake()
        {
            var lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer = lineRenderer != null ? lineRenderer : gameObject.AddComponent<LineRenderer>();
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            _lineRenderer.enabled = false;
            _lineRenderer.startWidth = 0.1f;
            _lineRenderer.endWidth = 0.1f;
        }

        private void FixedUpdate()
        {
            ClearDestroyedProjectiles();

            if (laser)
            {
                _activeHooks.ForEach(hook => hook.Instance.transform.position = hook.Destination);
                _lineRenderer.SetPosition(0, launchPoint.transform.position);
                return;
            }

            if (!_activeHooks.Any())
            {
                _lineRenderer.enabled = false;
            }

            _activeHooks.ForEach(hook =>
            {
                if (HookAttached(hook.Projectile))
                {
                    hook.Instance.gameObject.transform.position = hook.Destination;
                    return;
                }

                _lineRenderer.enabled = true;
                MoveProjectileTowardsDestination(hook);
                _lineRenderer.SetPosition(0, launchPoint.transform.position);
                _lineRenderer.SetPosition(1, hook.Instance.gameObject.transform.position);
            });
        }

        public void Launch()
        {
            if (_activeHooks.Any())
            {
                return;
            }

            var projectileObject = Instantiate(projectilePrefab);
            var projectile = projectileObject.GetComponent<IProjectile>();
            var projectileRb = projectileObject.GetComponent<Rigidbody>();

            projectileObject.transform.position = launchPoint.transform.position;
            projectileObject.transform.forward = _mainCamera.transform.forward;

            var target = GetTarget();
            var weaponOwner = gameObject.GetComponent<IWeapon>().GetOwner();
            projectile.Activate(target, weaponOwner);

            _activeHooks.Add(new ProjectileInstance
            {
                Instance = projectileObject,
                Projectile = projectile,
                Destination = target,
                InstanceRb = projectileRb
            });
        }

        public List<IProjectile> GetActiveProjectiles()
        {
            return _activeHooks
                .Select(instance => instance.Projectile)
                .ToList();
        }

        private void ClearDestroyedProjectiles()
        {
            _activeHooks.ToList().ForEach(hook =>
            {
                if (hook.Instance != null)
                {
                    return;
                }

                _activeHooks.Remove(hook);
            });
        }

        private Vector3 GetTarget()
        {
            Ray ray = RayCastUtil.GetRayToCenterOfScreen(_mainCamera);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, validDestinationLayers))
            {
                return hit.point;
            }

            return ray.GetPoint(maxDistance);
        }

        private void MoveProjectileTowardsDestination(ProjectileInstance projectileInstance)
        {
            var rb = projectileInstance.InstanceRb;
            var direction = (projectileInstance.Destination - rb.transform.position).normalized;
            rb.gameObject.transform.forward = direction;

            rb.AddForce(direction * (projectileInstance.Projectile.Speed * Time.fixedDeltaTime),
                ForceMode.Acceleration);
        }

        private bool HookAttached(IProjectile projectile)
        {
            return projectile.IsActive();
        }
    }
}