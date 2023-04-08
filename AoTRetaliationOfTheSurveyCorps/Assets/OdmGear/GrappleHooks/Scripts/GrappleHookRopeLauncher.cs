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
        private AudioSource _ropeSound;
        private Camera _mainCamera;
        private bool _shouldReleaseHook;

        private void Awake()
        {
            _hitDetection = GetComponent<IHitDetection>();
            _hookInput = GetComponent<IGrappleHookInput>();
            _lineRenderer = GetComponent<LineRenderer>();
            _ropeSound = GetComponent<AudioSource>();
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
            
            _ropeSound.Play();
            
            float hookTravelTime = distance / globalHookSettings.HookTravelSpeedInUnitsPerSeconds;
            
            StartCoroutine(_ropeAnimation.AnimateRope(_lineRenderer, hookTravelTime));
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