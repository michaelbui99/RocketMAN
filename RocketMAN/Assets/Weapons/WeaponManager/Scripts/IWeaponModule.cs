using UnityEngine;

namespace Weapons.WeaponManager.Scripts
{
    public interface IWeaponModule
    {
        public GameObject WeaponPrefab { get; set; }
        public string GetWeaponName();
        public void FireWeapon();
        public void ReloadWeapon();
        public Vector3? GetPositionOffSetVector();
    }
}