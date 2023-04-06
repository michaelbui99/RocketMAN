using UnityEngine;
using UnityEngine.InputSystem;

namespace OdmGear.GrappleHooks.Scripts
{
    public class GrappleHookInput : MonoBehaviour, IGrappleHookInput
    {
        public event IGrappleHookInput.OnLaunchHookInput OnLaunchHookEventInput;
        public event IGrappleHookInput.OnReleaseHookInput OnReleaseHookEventInput;

        public void OnHookInput(InputAction.CallbackContext value)
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
    }
}