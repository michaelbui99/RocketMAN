using System;
using System.Collections;
using UnityEngine;

namespace OdmGear.GrappleHooks
{
    public class HitDetection : MonoBehaviour, IHitDetection
    {
        [Header("References")]
        [SerializeField]
        private GrappleHookSettings globalSettings;

        public event IHitDetection.OnHit OnHitEvent;
        public event IHitDetection.OnMiss OnMissEvent;
        private IGrappleHookLauncher _launcher;
        private Camera _mainCamera;


        // Start is called before the first frame update
        private void Start()
        {
            _launcher = gameObject.GetComponent<IGrappleHookLauncher>();
            _launcher.OnLaunchHookEvent += OnLaunchHook;
            _mainCamera = Camera.main;
        }

        private void OnDestroy()
        {
            _launcher.OnLaunchHookEvent -= OnLaunchHook;
        }

        private void OnLaunchHook(Vector3 direction)
        {
            if (Camera.main is null)
            {
                throw new ApplicationException("No camera to use for ray");
            }

            Ray ray = new Ray(_mainCamera.ScreenPointToRay(Input.mousePosition).origin, direction.normalized);
            if (!Physics.Raycast(ray, out RaycastHit hit, globalSettings.MaxDistanceInUnits))
            {
                return;
            }

            StartCoroutine(PerformHookHitSimulation(hit.distance, direction.normalized));
        }

        private IEnumerator PerformHookHitSimulation(float distance, Vector3 direction)
        {
            yield return new WaitForSeconds(distance / globalSettings.HookTravelSpeedInUnitsPerSeconds);
            
            Ray ray = new Ray(_mainCamera.ScreenPointToRay(Input.mousePosition).origin, direction.normalized);
            if (!Physics.Raycast(ray, out RaycastHit hit, globalSettings.MaxDistanceInUnits))
            {
                OnMissEvent?.Invoke();
                yield break;
            }
            
            OnHitEvent?.Invoke(hit);
        }
    }
}