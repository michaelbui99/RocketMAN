using System;
using UnityEngine;

namespace OdmGear.GrappleHooks
{
    public interface IHitDetection
    {
        public delegate void OnHit(RaycastHit hit);
        public event OnHit OnHitEvent;
    }
}