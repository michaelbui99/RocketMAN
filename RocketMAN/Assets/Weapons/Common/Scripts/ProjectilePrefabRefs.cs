using UnityEngine;

namespace Weapons.Common.Scripts
{
    [CreateAssetMenu(fileName = "ProjectilePrefabRefs", menuName = "Projectile Prefab References")]
    public class ProjectilePrefabRefs: ScriptableObject
    {
        public GameObject RocketPrefab;
    }
}