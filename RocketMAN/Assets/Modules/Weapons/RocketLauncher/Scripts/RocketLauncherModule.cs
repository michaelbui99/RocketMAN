using Modules.Weapons.Common.Scripts;
using Modules.Weapons.WeaponManager.Scripts;
using UnityEngine;

namespace Modules.Weapons.RocketLauncher.Scripts
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