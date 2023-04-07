using System;
using UnityEngine;

namespace OdmGear.GrappleHooks.Scripts
{
    public class HookedBehaviour: MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Rigidbody bodyToPull;

        [Header("Settings")]
        [SerializeField]
        private float pullSpeed = 1f;

        private Vector3 _targetPoint;
        private IHitDetection _hitDetection;
        private bool _active = false;

        private void Awake()
        {
            _hitDetection = GetComponent<IHitDetection>();
        }

        private void LateUpdate()
        {
            if (!_active)
            {
                return;
            }
        }
    }
}