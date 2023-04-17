using UnityEngine;

namespace Weapons.Common.Scripts
{
    public interface IProjectile
    {
        public float Speed{ get; set; }
        public void OnCollision(Collision collision);
        public void Activate(Vector3 direction);
    }
}