using System;
using System.Collections.Generic;
using Modules.Weapons.Common.Scripts.Launchers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Modules.Weapons.GrapplingGun.Scripts
{
    public class Hook : MonoBehaviour, IProjectile
    {
        [field: Header("Settings")]
        [field: SerializeField]
        public float Speed { get; set; }

        private Vector3 _anchorPoint;
        private MeshRenderer _modelRenderer;
        private bool _attached = false;

        private GameObject _owner;
        private readonly List<SpringJoint> _jointInstances = new();
        private Vector3 _target;

        private void Awake()
        {
            _anchorPoint = Vector3.zero;
            _modelRenderer = GetComponentInChildren<MeshRenderer>();
        }

        private void Update()
        {
            if (_attached)
            {
                return;
            }

            var distanceToTarget = Vector3.Distance(gameObject.transform.position, _target);
            Debug.Log(distanceToTarget);
            if (distanceToTarget <= 1f)
            {
                AttachToPoint(_target);
            }
        }

        public void OnCollision(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                return;
            }

            AttachToPoint(collision.transform.position);
        }

        public void Activate(Vector3? destination, GameObject origin)
        {
            if (!destination.HasValue)
            {
                Destroy(gameObject);
            }

            _target = destination!.Value;
            if (origin != null)
            {
                _owner = origin;
            }
        }

        public void TriggerAlternateAction()
        {
            UnAttach();
        }

        public bool IsActive() => _attached;

        private void AttachToPoint(Vector3 point)
        {
            Debug.Log("ATTACHED");
            // NOTE: (mibui 2023-04-29) Using SpringJoints for swinging based on ADVANCED SWINGING in 9 MINUTES - Unity Tutorial
            _anchorPoint = point;
            _attached = true;

            var joint = _owner.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = _anchorPoint;

            float distanceFromPoint = Vector3.Distance(_owner.gameObject.transform.position, _anchorPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 4.5f;
            joint.massScale = 4.5f;
            _jointInstances.Add(joint);
            _modelRenderer.enabled = false;
        }

        private void UnAttach()
        {
            _attached = false;
            ClearJointInstances();
            
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }

        private void ClearJointInstances()
        {
            _jointInstances.ForEach(Destroy);
            _jointInstances.Clear();
        }

        private void OnDestroy()
        {
            UnAttach();
        }
    }
}