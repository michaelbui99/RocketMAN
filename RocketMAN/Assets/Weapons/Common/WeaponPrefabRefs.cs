using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(fileName = "WeaponPrefabRefs", menuName = "Weapon Prefab References")]
    public class WeaponPrefabRefs: ScriptableObject
    {
        public GameObject RocketLauncherPrefab;
    }
}