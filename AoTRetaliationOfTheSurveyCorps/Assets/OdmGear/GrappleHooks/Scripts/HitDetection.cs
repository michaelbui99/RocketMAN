using System;
using System.Collections;
using UnityEngine;

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

            _hasActiveCalculation = true;
            _cooldownHandler.StartCoolDown();
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            var direction = ray.direction.normalized;
            OnHookLaunchedEvent?.Invoke(direction);

            if (!Physics.Raycast(ray, out RaycastHit hit, globalSettings.MaxDistanceInUnits))
            {
                OnMissEvent?.Invoke();
                _hasActiveCalculation = false;
                return;
            }

            StartCoroutine(PerformHookHitSimulation(hit.distance, direction));
        }

        private IEnumerator PerformHookHitSimulation(float distance, Vector3 direction)
        {
            yield return new WaitForSeconds(distance / globalSettings.HookTravelSpeedInUnitsPerSeconds);

            Ray ray = new Ray(_mainCamera.ScreenPointToRay(Input.mousePosition).origin, direction.normalized);
            if (!Physics.Raycast(ray, out RaycastHit hit, globalSettings.MaxDistanceInUnits))
            {
                OnMissEvent?.Invoke();
                _hasActiveCalculation = false;
                yield break;
            }

            OnHitEvent?.Invoke(hit);
            _hasActiveCalculation = false;
        }
    }
}