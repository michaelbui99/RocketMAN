using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Odm.Scripts
{
    public class OdmInput: MonoBehaviour, IOdmInput
    {
        public event IOdmInput.OdmInputTrigger OnGasActivate;
        public event IOdmInput.OdmInputTrigger OnGasRelease;
        public event IOdmInput.OdmInputTrigger OnLeftHookFire;
        public event IOdmInput.OdmInputTrigger OnLeftHookRelease;
        public event IOdmInput.OdmInputTrigger OnRightHookFire;
        public event IOdmInput.OdmInputTrigger OnRightHookRelease;

        public void GasInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnGasActivate?.Invoke();
            }

            if (context.canceled)
            {
                OnGasRelease?.Invoke();
            }
        }

        public void RightHookInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnRightHookFire?.Invoke();
            }

            if (context.canceled)
            {
                OnRightHookRelease?.Invoke();
            }
        }
        
        public void LeftHookInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnLeftHookFire?.Invoke();
            }

            if (context.canceled)
            {
                OnLeftHookRelease?.Invoke();
            }
        }
    }
}