using Modules.Weapons.Common.Scripts;
using Modules.Weapons.Common.Scripts.Ammo;
using Modules.Weapons.Common.Scripts.Weapon;

namespace Modules.Weapons.RocketLauncher.Scripts
{
    public class RocketLauncher : WeaponBase
    {
        private void Start()
        {
            FireCooldown = 0.7f;
            Ammo.ReloadBehaviour = ReloadBehaviour.Continuous;
            Ammo.TotalReloadUnits = 20;
            Ammo.AmmoPerReloadUnit = 1;
            Ammo.ClipSize = 5;
            Ammo.AmmoState.CurrentAmmoCount = Ammo.ClipSize;
            Ammo.AmmoState.RemainingAmmoCount = 20;
            Ammo.ReloadTime = 0.5f;
        }
    }
}