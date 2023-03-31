using UnityEngine;
using UnityEngine.Serialization;

namespace OdmGear.GrappleHooks
{
    [CreateAssetMenu(fileName = "GrappleHookSettings", menuName = "Grapple Hook Settings")]
    public class GrappleHookSettings: ScriptableObject
    {
        public float MaxDistanceInUnits = 1f;
        public float HookTravelSpeedInUnitsPerSeconds = 1f;
        public LayerMask HookableLayers;
    }
}