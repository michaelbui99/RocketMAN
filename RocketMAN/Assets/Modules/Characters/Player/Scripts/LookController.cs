using System;
using Modules.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Modules.Characters.Player.Scripts
{
    public class LookController : MonoBehaviour
    {
        [Header(("References"))]
        [SerializeField]
        private GameObject weaponHolder;

        [SerializeField]
        private UnityEngine.Camera fpsCamera;

        [SerializeField]
        private GameObject followTarget;

        [SerializeField]
        private SensitivitySettings sensitivitySettings;


        private Rigidbody _rigidbody;
        private float _yRotation;
        private float _xRotation;
        private Vector3 _weaponOffSet;

        private IPausedGameObserver _pausedGameObserver;

        private void Awake()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _pausedGameObserver = GetComponentInChildren<IPausedGameObserver>();

            _pausedGameObserver.OnPauseEvent += EnableCursor;
            _pausedGameObserver.OnResumeEvent += DisableCursor;
        }

        public void OnLook(InputAction.CallbackContext value)
        {
            var lookDelta = value.ReadValue<Vector2>();
            _yRotation += lookDelta.x * HorizontalRotationSpeed() * 0.1f;
            _xRotation -= lookDelta.y * VerticalRotationSpeed() * 0.1f;
            _xRotation = Math.Clamp(_xRotation, -90f, 90);
        }

        private void LateUpdate()
        {
            if (_pausedGameObserver.GameIsPaused())
            {
                return;
            }

            _rigidbody.MoveRotation(Quaternion.Euler(0f, _yRotation, 0));
            followTarget.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            var cameraTransform = fpsCamera.transform;
            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, followTarget.transform.rotation,
                Time.deltaTime * 10);
            weaponHolder.transform.forward = cameraTransform.forward.normalized;
        }

        private void OnDestroy()
        {
            _pausedGameObserver.OnPauseEvent -= EnableCursor;
            _pausedGameObserver.OnResumeEvent -= DisableCursor;
        }

        private float VerticalRotationSpeed() => sensitivitySettings.VerticalMouseSensitivity;
        private float HorizontalRotationSpeed() => sensitivitySettings.HorizontalMouseSensitivity;

        private void EnableCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void DisableCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}