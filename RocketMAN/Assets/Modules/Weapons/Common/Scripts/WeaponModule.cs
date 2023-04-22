using UnityEngine;

namespace Modules.Weapons.Common.Scripts
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