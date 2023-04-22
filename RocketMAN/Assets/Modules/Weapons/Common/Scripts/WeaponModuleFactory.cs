using System;
using Modules.Weapons.RocketLauncher.Scripts;
using Modules.Weapons.WeaponManager.Scripts;
using UnityEngine;

namespace Modules.Weapons.Common.Scripts
{
    [CreateAssetMenu]
    public class WeaponModuleFactory : ScriptableObject
    {
        public WeaponModule RocketLauncherModule;
        public WeaponModule Create(string weapon)
        {
            return weapon switch
            {
                SupportedWeaponModules.RocketLauncher => RocketLauncherModule,
                _ => throw new ArgumentException("Not supported")
            };
        }

        public WeaponModule GetDefault()
        {
            return RocketLauncherModule;
        }
    }
}