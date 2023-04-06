using UnityEngine;

namespace OdmGear.GrappleHooks.Scripts
{
    [CreateAssetMenu(fileName = "GrappleHookSettings", menuName = "Grapple Hook Settings")]
    public class GrappleHookSettings : ScriptableObject
    {
        public float MaxDistanceInUnits = 1f;
        public float HookTravelSpeedInUnitsPerSeconds = 1f;
        public float HookCooldownInSeconds = 1f;
        public LayerMask HookableLayers;
    }
}