using UnityEngine;
using UnityEngine.InputSystem;
using Weapons.Common;
using Weapons.Common.Scripts;

namespace Weapons.WeaponManager.Scripts
{
    public class WeaponInput: MonoBehaviour, IWeaponInput
    {
        public event IWeaponInput.FireWeapon OnFireWeapon;
        public event IWeaponInput.SwitchWeapon OnSwitchWeapon;

        private void Awake()
        {
            OnSwitchWeapon?.Invoke(SupportedWeaponModules.RocketLauncher);
        }

        public void FireWeaponInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnFireWeapon?.Invoke();
            }
        }

        public void SwitchToRocketLauncher(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnSwitchWeapon?.Invoke(SupportedWeaponModules.RocketLauncher);
            }
        }
    }
}