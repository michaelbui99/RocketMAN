using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.Scripts
{
    public class LookController : MonoBehaviour
    {
        [Header(("References"))]
        [SerializeField]
        private GameObject player;

        [SerializeField]
        private GameObject weaponHolder;
        [SerializeField]
        private Camera fpsCamera;

        [SerializeField]
        private GameObject followTarget;

        [Header("Custom settings")]
        [SerializeField]
        private float verticalRotationSpeed = 1f;

        [SerializeField]
        private float horizontalRotationSpeed = 1f;

        private Rigidbody _rigidbody;
        private float _yRotation;
        private float _xRotation;
        private Vector3 _weaponOffSet;

        private void Awake()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
        }

        public void OnLook(InputAction.CallbackContext value)
        {
            var lookDelta = value.ReadValue<Vector2>();
            _yRotation += lookDelta.x * horizontalRotationSpeed * 0.1f;
            _xRotation -= lookDelta.y * verticalRotationSpeed * 0.1f;
            _xRotation = Math.Clamp(_xRotation, -90f, 90);
        }

        private void LateUpdate()
        {
            _rigidbody.MoveRotation(Quaternion.Euler(0f, _yRotation, 0));
            followTarget.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            var cameraTransform = fpsCamera.transform;
            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, followTarget.transform.rotation, Time.deltaTime*10);
            weaponHolder.transform.forward = cameraTransform.forward.normalized;
        }
    }
}