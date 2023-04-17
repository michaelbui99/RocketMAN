using UnityEngine;

namespace Weapons.Common.Scripts
{
    [CreateAssetMenu(fileName = "WeaponPrefabRefs", menuName = "Weapon Prefab References")]
    public class WeaponPrefabRefs: ScriptableObject
    {
        public GameObject RocketLauncherPrefab;
    }
}