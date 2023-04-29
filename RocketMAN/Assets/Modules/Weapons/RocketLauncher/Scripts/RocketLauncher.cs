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
        }
    }
}