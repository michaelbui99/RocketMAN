using Modules.Weapons.Common.Scripts.Ammo;
using UnityEngine;

namespace Modules.Weapons.Common.Scripts.Weapon
{
    [CreateAssetMenu(menuName = "Weapon Module")]
    public class WeaponModule: ScriptableObject
    {
        [Header("Prefab")]
        public GameObject WeaponPrefab;
        
        [Header("Names")]
        public string InternalWeaponName;
        public string DisplayName;
        
        [Header("Render Position Settings")]
        public Vector3 WeaponPositionOffset;


        [Header("Ammo Settings")]
        public ReloadBehaviour ReloadBehaviour;

        public int ClipSize;
        public int TotalReloadUnits;
        public int AmmoPerReloadUnit;
        public float ReloadTime;
        public bool UnlimitedAmmo = false;
    }
}