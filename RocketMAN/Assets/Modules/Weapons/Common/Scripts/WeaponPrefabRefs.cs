using UnityEngine;

namespace Modules.Weapons.Common.Scripts
{
    [CreateAssetMenu(fileName = "WeaponPrefabRefs", menuName = "Weapon Prefab References")]
    public class WeaponPrefabRefs: ScriptableObject
    {
        public GameObject RocketLauncherPrefab;
    }
}