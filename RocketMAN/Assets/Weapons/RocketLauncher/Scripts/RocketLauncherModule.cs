using System;
using UnityEngine;
using Weapons.Common;
using Weapons.Common.Scripts;
using Weapons.WeaponManager;
using Weapons.WeaponManager.Scripts;

namespace Weapons.RocketLauncher.Scripts
{
    public class RocketLauncherModule : IWeaponModule
    {
        public GameObject WeaponPrefab { get; set; }

        public RocketLauncherModule()
        {
            WeaponPrefab = ((WeaponPrefabRefs) Resources.Load("WeaponPrefabRefs")).RocketLauncherPrefab;
        }

        public string GetWeaponName()
        {
            return SupportedWeaponModules.RocketLauncher;
        }

        public Vector3? GetPositionOffSetVector()
        {
            throw new System.NotImplementedException();
        }
    }
}