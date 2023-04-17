using UnityEngine;

namespace Weapons.Common
{
    [CreateAssetMenu(fileName = "ProjectilePrefabRefs", menuName = "Projectile Prefab References")]
    public class ProjectilePrefabRefs: ScriptableObject
    {
        public GameObject RocketPrefab;
    }
}