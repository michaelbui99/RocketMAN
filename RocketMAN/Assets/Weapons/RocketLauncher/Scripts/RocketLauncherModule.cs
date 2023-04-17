using UnityEngine;
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

        public void FireWeapon()
        {
            Debug.Log("ROCKET LAUNCHER FIRED");
        }

        public void ReloadWeapon()
        {
            throw new System.NotImplementedException();
        }

        public Vector3? GetPositionOffSetVector()
        {
            throw new System.NotImplementedException();
        }
    }
}