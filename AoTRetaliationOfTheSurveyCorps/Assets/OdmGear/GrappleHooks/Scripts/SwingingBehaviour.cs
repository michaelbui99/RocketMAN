using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

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
        private float? _initialAttachDistance;

        [CanBeNull]
        private SpringJoint _joint;

        private readonly List<SpringJoint> _jointInstances = new();

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
            AdjustJointDistances(_joint, _anchorPoint.Value);
        }

        private void AttachToAnchorPoint(RaycastHit hit)
        {
            ClearJointInstances();
            _anchorPoint = hit.point;
            _initialAttachDistance = GetDistanceFromRigidbodyToAnchorPoint(rigidbodyToActOn, _anchorPoint.Value);

            _joint = rigidbodyToActOn.AddComponent<SpringJoint>();
            _jointInstances.Add(_joint);
            _joint!.autoConfigureConnectedAnchor = false;
            _joint!.anchor = _anchorPoint.Value;

            AdjustJointDistances(_joint, _anchorPoint.Value);

            _joint.spring = globalHookSettings.JointSpring;
            _joint.massScale = globalHookSettings.JointMassScale;
            _joint.damper = globalHookSettings.JointDamper;
        }

        private void DetachFromAnchorPoint()
        {
            if (_joint is not null)
            {
                ClearJointInstances();
            }

            if (_anchorPoint.HasValue)
            {
                _anchorPoint = null;
            }

            _initialAttachDistance = null;
        }

        private void WheelIn()
        {
            if (_joint is null || !_anchorPoint.HasValue)
            {
                return;
            }

            PullRigidbodyTowardsAnchor(rigidbodyToActOn, _anchorPoint.Value, _joint,
                globalHookSettings.HookPullForce * globalHookSettings.WheelInOutFactor);
            AdjustJointDistances(_joint, _anchorPoint.Value);
        }

        private void WheelOut()
        {
            if (_joint is null || !_anchorPoint.HasValue)
            {
                return;
            }

            PushRigidbodyAwayFromAnchor(rigidbodyToActOn, _anchorPoint.Value, _joint,
                globalHookSettings.HookPullForce * globalHookSettings.WheelInOutFactor);
            AdjustJointDistances(_joint, _anchorPoint.Value);
        }

        private void AdjustJointDistances(SpringJoint joint, Vector3 anchorPoint)
        {
            var distanceToAnchorPoint = GetDistanceFromRigidbodyToAnchorPoint(rigidbodyToActOn, anchorPoint);
            joint.maxDistance = distanceToAnchorPoint * globalHookSettings.RopeSlackFactor;
            if (_initialAttachDistance.HasValue)
            {
                joint.minDistance = -_initialAttachDistance.Value;
            }
        }

        private float GetDistanceFromRigidbodyToAnchorPoint(Rigidbody rb, Vector3 anchorPoint)
        {
            return Vector3.Distance(rb.transform.position, anchorPoint);
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
        }

        private void PushRigidbodyAwayFromAnchor(Rigidbody rb, Vector3 anchorPoint, SpringJoint joint, float pushForce)
        {
            if (rb is null || joint is null)
            {
                return;
            }

            var pullDirection = -(anchorPoint - rigidbodyToActOn.transform.position).normalized;
            rb.AddForce(pullDirection * pushForce);
        }

        private void ClearJointInstances()
        {
            _jointInstances.ForEach(Destroy);
            _jointInstances.Clear();
        }
    }
}