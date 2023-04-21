using Modules.Weapons.Common.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Weapons.WeaponManager.Scripts
{
    public class WeaponInput: MonoBehaviour, IWeaponInput
    {
        public event IWeaponInput.SimpleWeaponEventTrigger OnFireWeapon;
        public event IWeaponInput.SimpleWeaponEventTrigger OnReloadWeapon;
        public event IWeaponInput.SwitchWeapon OnSwitchWeapon;

        private void Awake()
        {
            OnSwitchWeapon?.Invoke(SupportedWeaponModules.RocketLauncher);
        }

        public void FireWeaponInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnFireWeapon?.Invoke();
            }
        }

        public void SwitchToRocketLauncher(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnSwitchWeapon?.Invoke(SupportedWeaponModules.RocketLauncher);
            }
        }

        public void ReloadWeapon(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnReloadWeapon?.Invoke();
            }
        }
    }
}