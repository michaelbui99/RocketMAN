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
            var module = Modules.First(m => m.InternalWeaponName == weapon);
            if (module == null)
            {
                throw new NotSupportedException("Module is not supported");
            }

            return module;
        }

        public WeaponModule GetDefault()
        {
            return Modules.First(m => m.InternalWeaponName == SupportedWeaponModules.RocketLauncher);
        }
    }
}