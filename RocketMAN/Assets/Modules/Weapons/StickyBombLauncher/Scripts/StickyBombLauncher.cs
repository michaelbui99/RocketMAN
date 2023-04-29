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
        }
    }
}