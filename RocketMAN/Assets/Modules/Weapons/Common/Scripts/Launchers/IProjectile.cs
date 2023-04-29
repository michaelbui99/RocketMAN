using JetBrains.Annotations;
using UnityEngine;

namespace Modules.Weapons.Common.Scripts.Launchers
{
    public interface IProjectile
    {
        public float Speed{ get; set; }
        public void OnCollision(Collision collision);
        public void Activate(Vector3? destination, [CanBeNull] GameObject origin = null);
        public void TriggerAlternateAction();

        public bool IsActive();
    }
}