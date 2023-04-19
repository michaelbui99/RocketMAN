using UnityEngine;

namespace Modules.Weapons.Common.Scripts
{
    [CreateAssetMenu(fileName = "ProjectilePrefabRefs", menuName = "Projectile Prefab References")]
    public class ProjectilePrefabRefs: ScriptableObject
    {
        public GameObject RocketPrefab;
    }
}