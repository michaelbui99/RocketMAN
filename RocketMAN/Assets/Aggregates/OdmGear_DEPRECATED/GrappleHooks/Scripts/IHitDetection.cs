using UnityEngine;

namespace OdmGear.GrappleHooks.Scripts
{
    public interface IHitDetection
    {
        public delegate void OnHit(RaycastHit hit);
        public event OnHit OnHitEvent;
        
        public delegate void OnMiss();
        public event OnMiss OnMissEvent;

        public delegate void OnHookLaunched(Vector3 direction, float distance);
        public event OnHookLaunched OnHookLaunchedEvent;
    }
}