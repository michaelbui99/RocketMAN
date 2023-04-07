using System;
using OdmGear.GrappleHooks.Scripts;
using UnityEngine;

namespace OdmGear.GrappleHooks
{
    public class GrappleHookRopeLauncher : MonoBehaviour

    {
        [Header("References")]
        [SerializeField]
        private GameObject launchPoint;

        [SerializeField]
        private GrappleHookSettings globalHookSettings;

        private IHitDetection _hitDetection;
        private IGrappleHookInput _hookInput;
        private HookCooldownHandler _cooldownHandler;
        private GasExpenditureController _gasExpenditureController;
        private LineRenderer _lineRenderer;
        private RopeAnimation _ropeAnimation;
        private Camera _mainCamera;
        private bool _shouldReleaseHook;
        private bool _launching;

        private void Awake()
        {
            _hitDetection = gameObject.GetComponent<IHitDetection>();
            _hookInput = gameObject.GetComponent<IGrappleHookInput>();
            _lineRenderer = gameObject.GetComponent<LineRenderer>();
            _ropeAnimation = new RopeAnimation();
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            _hitDetection.OnHookLaunchedEvent += LaunchHook;
            _hitDetection.OnHitEvent += AttachHook;
            _hitDetection.OnMissEvent += ResetHook;
            _hookInput.OnReleaseHookEventInput += ReleaseHook;
            _lineRenderer.enabled = false;
        }

        private void OnDestroy()
        {
            _hitDetection.OnHookLaunchedEvent -= LaunchHook;
            _hitDetection.OnHitEvent -= AttachHook;
            _hitDetection.OnMissEvent -= ResetHook;
            _hookInput.OnReleaseHookEventInput -= ReleaseHook;
        }

        private void Update()
        {
            if (_shouldReleaseHook)
            {
                ResetHook();
            }
        }

        private void LateUpdate()
        {
            launchPoint.transform.forward = _mainCamera.transform.forward;
            _lineRenderer.SetPosition(0, launchPoint.transform.position);
        }

        private void LaunchHook(Vector3 direction, float distance)
        {
            _lineRenderer.enabled = true;
            _shouldReleaseHook = false;
            _lineRenderer.SetPosition(1, direction);
            float duration = distance / globalHookSettings.HookTravelSpeedInUnitsPerSeconds;
            StartCoroutine(_ropeAnimation.AnimateRope(_lineRenderer, duration));
        }

        private void AttachHook(RaycastHit hit)
        {
            _lineRenderer.SetPosition(1, hit.point);
        }

        private void ResetHook()
        {
            _lineRenderer.enabled = false;
        }

        private void ReleaseHook()
        {
            _shouldReleaseHook = true;
        }
    }
}