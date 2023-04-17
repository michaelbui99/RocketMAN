using UnityEngine;

namespace Weapons.WeaponManager.Scripts
{
    public interface IWeaponModule
    {
        public string GetWeaponName();
        public void FireWeapon();
        public void ReloadWeapon();
        public GameObject WeaponPrefab{ get; set; }
        public Vector3? GetPositionOffSetVector();
    }
}