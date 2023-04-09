using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace OdmGear.GrappleHooks.Scripts
{
    public class SwingingBehaviour : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Rigidbody rigidbodyToActOn;

        [SerializeField]
        private GrappleHookSettings globalHookSettings;


        private IHitDetection _hitDetection;
        private IGrappleHookInput _grappleHookInput;
        private Vector3? _anchorPoint;

        [CanBeNull]
        private SpringJoint _joint;


        private void Awake()
        {
            _hitDetection = GetComponent<IHitDetection>();
            _grappleHookInput = GetComponent<IGrappleHookInput>();
        }

        private void Start()
        {
            _hitDetection.OnHitEvent += AttachToAnchorPoint;
            _grappleHookInput.OnReleaseHookEventInput += DetachFromAnchorPoint;
            _grappleHookInput.OnWheelInEvent += WheelIn;
            _grappleHookInput.OnWheelOutEvent += WheelOut;
        }

        private void OnDestroy()
        {
            _hitDetection.OnHitEvent -= AttachToAnchorPoint;
            _grappleHookInput.OnReleaseHookEventInput -= DetachFromAnchorPoint;
            _grappleHookInput.OnWheelInEvent -= WheelIn;
            _grappleHookInput.OnWheelOutEvent -= WheelOut;
        }

        private void FixedUpdate()
        {
            if (_joint is null || !_anchorPoint.HasValue)
            {
                return;
            }

            PullRigidbodyTowardsAnchor(rigidbodyToActOn, _anchorPoint.Value, _joint, globalHookSettings.HookPullForce);
        }

        private void AttachToAnchorPoint(RaycastHit hit)
        {
            _anchorPoint = hit.point;

            _joint = rigidbodyToActOn.AddComponent<SpringJoint>();
            _joint!.autoConfigureConnectedAnchor = false;
            _joint!.anchor = _anchorPoint.Value;

            AdjustJointDistances(_joint, _anchorPoint.Value);

            _joint.spring = globalHookSettings.JointSpring;
            _joint.massScale = globalHookSettings.JointMassScale;
            _joint.damper = globalHookSettings.JointDamper;
        }

        private void DetachFromAnchorPoint()
        {
            if (_anchorPoint.HasValue)
            {
                _anchorPoint = null;
            }

            Destroy(_joint);
            _joint = null;
        }

        private void WheelIn()
        {
            if (_joint is null || !_anchorPoint.HasValue)
            {
                return;
            }

            PullRigidbodyTowardsAnchor(rigidbodyToActOn, _anchorPoint.Value, _joint,
                globalHookSettings.HookPullForce * globalHookSettings.WheelInOutFactor);
        }

        private void WheelOut()
        {
            if (_joint is null || !_anchorPoint.HasValue)
            {
                return;
            }

            PushRigidbodyAwayFromAnchor(rigidbodyToActOn, _anchorPoint.Value, _joint,
                globalHookSettings.HookPullForce * globalHookSettings.WheelInOutFactor);
        }

        private void AdjustJointDistances(SpringJoint joint, Vector3 anchorPoint)
        {
            var distanceToAnchorPoint = Vector3.Distance(rigidbodyToActOn.transform.position, anchorPoint);
            joint.maxDistance = distanceToAnchorPoint * 0.8f;
            joint.minDistance = distanceToAnchorPoint * globalHookSettings.RopeSlackFactor;
        }

        private void PullRigidbodyTowardsAnchor(Rigidbody rb, Vector3 anchorPoint, SpringJoint joint,
            float pullForce)
        {
            if (rb is null || joint is null)
            {
                return;
            }

            var pullDirection = (anchorPoint - rigidbodyToActOn.transform.position).normalized;
            rb.AddForce(pullDirection * pullForce);
            AdjustJointDistances(joint, anchorPoint);
        }

        private void PushRigidbodyAwayFromAnchor(Rigidbody rb, Vector3 anchorPoint, SpringJoint joint, float pushForce)
        {
            if (rb is null || joint is null)
            {
                return;
            }

            var pullDirection = -(anchorPoint - rigidbodyToActOn.transform.position).normalized;
            rb.AddForce(pullDirection * pushForce);
            AdjustJointDistances(joint, anchorPoint);
        }
    }
}