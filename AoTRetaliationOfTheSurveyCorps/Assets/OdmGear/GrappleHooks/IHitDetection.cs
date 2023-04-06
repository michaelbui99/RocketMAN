using System;
using UnityEngine;

namespace OdmGear.GrappleHooks
{
    public interface IHitDetection
    {
        public delegate void OnHit(RaycastHit hit);
        public event OnHit OnHitEvent;
        
        public delegate void OnMiss();
        public event OnMiss OnMissEvent;
    }
}