using Modules.Weapons.Common.Scripts;
using Modules.Weapons.Common.Scripts.Weapon;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Weapons.WeaponManager.Scripts
{
    public class WeaponInput : MonoBehaviour, IWeaponInput
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
            OnFireWeapon?.Invoke();
        }

        public void SwitchToRocketLauncher(InputAction.CallbackContext context)
        {
            OnSwitchWeapon?.Invoke(SupportedWeaponModules.RocketLauncher);
        }

        public void SwitchToStickyBombLauncher(InputAction.CallbackContext context)
        {
            OnSwitchWeapon?.Invoke(SupportedWeaponModules.StickyBombLauncher);
        }

        public void ReloadWeapon(InputAction.CallbackContext context)
        {
            OnReloadWeapon?.Invoke();
        }
    }
}