using UnityEngine;
using UnityEngine.InputSystem;

namespace OdmGear.GrappleHooks
{
    public class GrappleHookInput : MonoBehaviour, IGrappleHookInput
    {
        public event IGrappleHookInput.OnLaunchHookInput OnLaunchHookEventInput;

        public void OnHookInput(InputAction.CallbackContext value)
        {
            OnLaunchHookEventInput?.Invoke();
        }
    }
}