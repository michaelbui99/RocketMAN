using System;
using UnityEngine;

namespace OdmGear.GrappleHooks
{
    public class GrappleHookLauncher : MonoBehaviour, IGrappleHookLauncher
    {
        [Header("References")]
        [SerializeField]
        private GameObject hookPrefab;

        [SerializeField]
        private GameObject launchPoint;

        private IGrappleHookInput _grappleHookInput;

        private Camera _mainCamera;

        public event IGrappleHookLauncher.OnLaunchHook OnLaunchHookEvent;

        private void Awake()
        {
            _grappleHookInput = gameObject.GetComponent<IGrappleHookInput>();
        }

        private void Start()
        {
            _mainCamera = Camera.main;

            _grappleHookInput.OnLaunchHookEventInput += LaunchHook;
        }

        private void LaunchHook()
        {
            OnLaunchHookEvent?.Invoke(_mainCamera.ScreenPointToRay(Input.mousePosition).direction);
        }

        public void StopHook()
        {
            Debug.Log("Stop hook");
        }
    }
}