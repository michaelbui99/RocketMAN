using System;
using UnityEngine;

namespace OdmGear.GrappleHooks
{
    public class GrappleHookMediator : MonoBehaviour
    {
        private IGrappleHookLauncher _grappleHookLauncher;
        private IHitDetection _hitDetection;

        private void Awake()
        {
            _grappleHookLauncher = gameObject.GetComponent<IGrappleHookLauncher>();
            _hitDetection = gameObject.GetComponent<IHitDetection>();
        }

        private void Start()
        {
            _hitDetection.OnHitEvent += OnHitDetected;
        }

        private void OnDestroy()
        {
            _hitDetection.OnHitEvent -= OnHitDetected;
        }

        private void OnHitDetected(RaycastHit hit)
        {
            Debug.Log($"HIT! {JsonUtility.ToJson(hit)}");
            _grappleHookLauncher.StopHook();
        }
    }
}