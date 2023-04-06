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

        private IHitDetection _hitDetection;
        private IGrappleHookInput _hookInput;
        private HookCooldownHandler _cooldownHandler;
        private LineRenderer _lineRenderer;
        private bool _shouldReleaseHook;

        private void Awake()
        {
            _hitDetection = gameObject.GetComponent<IHitDetection>();
            _hookInput = gameObject.GetComponent<IGrappleHookInput>();
            _lineRenderer = gameObject.GetComponent<LineRenderer>();
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
            _lineRenderer.SetPosition(0, launchPoint.transform.position);
        }

        private void LaunchHook(Vector3 direction)
        {
            _lineRenderer.enabled = true;
            _shouldReleaseHook = false;
            _lineRenderer.SetPosition(1, direction * 3);
        }

        private void AttachHook(RaycastHit hit)
        {
            _lineRenderer.SetPosition(1, hit.transform.position);
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