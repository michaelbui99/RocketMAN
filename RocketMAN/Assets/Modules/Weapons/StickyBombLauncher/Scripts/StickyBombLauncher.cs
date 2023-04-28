using System;
using Modules.Weapons.Common.Scripts.Ammo;
using Modules.Weapons.Common.Scripts.Weapon;
using Modules.Weapons.RocketLauncher.Scripts;

namespace Modules.Weapons.StickyBombLauncher.Scripts
{
    public class StickyBombLauncher: WeaponBase
    {
        private void Start()
        {
            FireCooldown = 0.7f;
            Ammo.ReloadBehaviour = ReloadBehaviour.Discrete;
            Ammo.TotalReloadUnits = 2;
            Ammo.AmmoPerReloadUnit = 2;
            Ammo.ClipSize = 2;
            Ammo.AmmoState.CurrentAmmoCount = Ammo.ClipSize;
            Ammo.AmmoState.RemainingAmmoCount = 2;
            Ammo.ReloadTime = 1f;
        }
    }
}