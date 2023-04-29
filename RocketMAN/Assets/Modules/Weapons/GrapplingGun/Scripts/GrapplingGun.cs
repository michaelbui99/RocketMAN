using System;
using System.Linq;
using Modules.Weapons.Common.Scripts.Weapon;

namespace Modules.Weapons.GrapplingGun.Scripts
{
    public class GrapplingGun : WeaponBase
    {
        private void Start()
        {
            FireCooldown = 1f;
            PreventFirePredicate = _ => ProjectileLauncher.GetActiveProjectiles().Any();
        }
    }
}