using UnityEngine;

namespace Modules.Weapons.WeaponManager.Scripts
{
    public interface IWeaponModule 
    {
        public GameObject WeaponPrefab { get; set; }
        public string GetWeaponName();
        public Vector3? GetPositionOffSetVector();
    }
}