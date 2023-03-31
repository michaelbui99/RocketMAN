using System;
using UnityEngine;

namespace OdmGear.GrappleHooks
{
    public interface IGrappleHookLauncher
    {
        public delegate void OnLaunchHook(Vector3 direction);

        public event OnLaunchHook OnLaunchHookEvent;

        public void StopHook();
    }
}