using UnityEngine;
using UnityEngine.InputSystem;

namespace OdmGear.GrappleHooks.Scripts
{
    public class GrappleHookInput : MonoBehaviour, IGrappleHookInput
    {
        public event IGrappleHookInput.OnLaunchHookInput OnLaunchHookEventInput;
        public event IGrappleHookInput.OnReleaseHookInput OnReleaseHookEventInput;
        public event IGrappleHookInput.OnWheelInInput OnWheelInEvent;
        public event IGrappleHookInput.OnWheelOutInput OnWheelOutEvent;

        public void OnLaunchHookInput(InputAction.CallbackContext value)
        {
            if (value.started)
            {
                OnLaunchHookEventInput?.Invoke();
            }

            if (value.canceled)
            {
                OnReleaseHookEventInput?.Invoke();
            }
        }

        public void OnWheelInInput(InputAction.CallbackContext value)
        {
            OnWheelInEvent?.Invoke();
        }

        public void OnWheelOutInput(InputAction.CallbackContext value)
        {
            OnWheelOutEvent?.Invoke();
        }
    }
}