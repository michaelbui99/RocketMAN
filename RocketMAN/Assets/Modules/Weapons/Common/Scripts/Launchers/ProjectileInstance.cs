using UnityEngine;

namespace Modules.Weapons.Common.Scripts.Launchers
{
    public class ProjectileInstance
    {
        public IProjectile Projectile{ get; set; }
        public Rigidbody InstanceRb{ get; set; }
        public GameObject Instance{ get; set; }
        public Vector3 Destination{ get; set; }
    }
}