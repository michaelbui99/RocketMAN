using UnityEngine;
using Utility;

namespace Modules.Weapons.Common.Scripts
{
    public class ProjectileLauncher : MonoBehaviour, IProjectileLauncher
    {
        [Header("References")]
        [SerializeField]
        private GameObject launchPoint;

        [SerializeField]
        private GameObject projectilePrefab;
        
        [Header("Settings")]
        [SerializeField]
        private LayerMask validDestinationTargetLayers;

        private UnityEngine.Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = UnityEngine.Camera.main;
        }

        public void Launch()
        {
            Ray ray = RayCastUtil.GetRayToCenterOfScreen(_mainCamera);
            var target = Physics.Raycast(ray, out RaycastHit hit, validDestinationTargetLayers) ? hit.point : ray.GetPoint(1000);

            var projectileObject = Instantiate(projectilePrefab);
            projectileObject.transform.position = launchPoint.transform.position;
            projectileObject.transform.forward = _mainCamera.transform.forward;
            var projectile = projectileObject.GetComponent<IProjectile>();
            projectile.Activate(target);
        }
    }
}