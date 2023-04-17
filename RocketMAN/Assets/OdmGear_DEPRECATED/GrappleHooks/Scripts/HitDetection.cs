using System;
using System.Collections;
using UnityEngine;
using Utility;

namespace OdmGear.GrappleHooks.Scripts
{
    public class HitDetection : MonoBehaviour, IHitDetection
    {
        [Header("References")]
        [SerializeField]
        private GrappleHookSettings globalSettings;

        public event IHitDetection.OnHit OnHitEvent;
        public event IHitDetection.OnMiss OnMissEvent;
        public event IHitDetection.OnHookLaunched OnHookLaunchedEvent;

        private IGrappleHookInput _hookInput;
        private Camera _mainCamera;
        private bool _hasActiveCalculation = false;
        private HookCooldownHandler _cooldownHandler;

        private DateTime _hookLaunchedTime;

        private void Awake()
        {
            _hookInput = gameObject.GetComponent<IGrappleHookInput>();
            _mainCamera = Camera.main;
            _cooldownHandler = gameObject.GetComponent<HookCooldownHandler>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            _hookInput.OnLaunchHookEventInput += OnLaunchHook;
        }

        private void OnDestroy()
        {
            _hookInput.OnLaunchHookEventInput -= OnLaunchHook;
        }

        private void OnLaunchHook()
        {
            if (_hasActiveCalculation || _cooldownHandler.CooldownActive())
            {
                return;
            }

            if (Camera.main is null)
            {
                throw new ApplicationException("No camera to use for ray");
            }

            Ray ray = RayCastUtil.GetRayToCenterOfScreen(_mainCamera);

            if (!Physics.Raycast(ray, out RaycastHit hit, globalSettings.MaxDistanceInUnits,
                    globalSettings.HookableLayers))
            {
                if (!SmartAim.Scripts.SmartAim.SmartAimHit(ray, out hit, globalSettings))
                {
                    OnHookLaunchedEvent?.Invoke(ray.direction, globalSettings.MaxDistanceInUnits);
                    StartCoroutine(PerformHookTravelSimulation(globalSettings.MaxDistanceInUnits, null));
                    return;
                }

                OnHookLaunchedEvent?.Invoke(hit.point, hit.distance);
                StartCoroutine(PerformHookTravelSimulation(hit.distance, hit));
            }

            OnHookLaunchedEvent?.Invoke(hit.point, hit.distance);
            StartCoroutine(PerformHookTravelSimulation(hit.distance, hit));
        }

        private IEnumerator PerformHookTravelSimulation(float distance, RaycastHit? hit)
        {
            float travelTime = distance / globalSettings.HookTravelSpeedInUnitsPerSeconds;
            _hasActiveCalculation = true;
            _cooldownHandler.StartCooldown(travelTime);
            yield return new WaitForSeconds(travelTime);

            if (!hit.HasValue)
            {
                OnMissEvent?.Invoke();
                _hasActiveCalculation = false;
                yield break;
            }


            OnHitEvent?.Invoke(hit.Value);
            _hasActiveCalculation = false;
        }
    }
}