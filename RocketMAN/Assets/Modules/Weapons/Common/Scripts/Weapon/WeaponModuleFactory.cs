using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Modules.Weapons.Common.Scripts.Weapon
{
    [CreateAssetMenu]
    public class WeaponModuleFactory : ScriptableObject
    {
        public List<WeaponModule> Modules;

        public WeaponModule Create(string weapon)
        {
            return weapon switch
            {
                SupportedWeaponModules.RocketLauncher => Modules.First(m =>
                    m.InternalWeaponName == SupportedWeaponModules.RocketLauncher),
                
                SupportedWeaponModules.StickyBombLauncher => Modules.First(m =>
                    m.InternalWeaponName == SupportedWeaponModules.StickyBombLauncher),
                
                _ => Modules.FirstOrDefault(m => m.InternalWeaponName == weapon)
                    ? Modules.FirstOrDefault(m => m.InternalWeaponName == weapon)
                    : throw new ArgumentException("Module is not supported")
            };
        }

        public WeaponModule GetDefault()
        {
            return Modules.First(m => m.InternalWeaponName == SupportedWeaponModules.RocketLauncher);
        }
    }
}