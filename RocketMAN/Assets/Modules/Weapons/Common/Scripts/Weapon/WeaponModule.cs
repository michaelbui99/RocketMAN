using UnityEngine;

namespace Modules.Weapons.Common.Scripts.Weapon
{
    [CreateAssetMenu(menuName = "Weapon Module")]
    public class WeaponModule: ScriptableObject
    {
        public GameObject WeaponPrefab;
        public string InternalWeaponName;
        public string DisplayName;
        public Vector3 WeaponPositionOffset;
    }
}