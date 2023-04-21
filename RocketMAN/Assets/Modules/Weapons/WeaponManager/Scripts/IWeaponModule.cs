using UnityEngine;

namespace Modules.Weapons.WeaponManager.Scripts
{
    /// <summary>
    /// Used for exporting the correct prefabs for a weapon and describes metadata about the weapon
    /// </summary>
    public interface IWeaponModule
    {
        public GameObject WeaponPrefab { get; set; }
        public string GetWeaponName();
        public Vector3? GetPositionOffSetVector();
    }
}