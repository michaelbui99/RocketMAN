using UnityEngine;
using UnityEngine.Serialization;

namespace OdmGear.GrappleHooks.Scripts
{
    [CreateAssetMenu(fileName = "GrappleHookSettings", menuName = "Grapple Hook Settings")]
    public class GrappleHookSettings : ScriptableObject
    {
        // General
        public float MaxDistanceInUnits = 1f;
        public float HookTravelSpeedInUnitsPerSeconds = 1f;
        public LayerMask HookableLayers;

        // Gas Costs
        public float LaunchHookGasCost = 1f;
        public float OnHookPropulsionGasCost = 1f;

        // Cooldowns
        public float MaxHookCooldownInSeconds => MaxDistanceInUnits / HookTravelSpeedInUnitsPerSeconds;

        // Swinging
        public float JointSpring = 5f;
        public float JointDamper = 8f;
        public float JointMassScale = 10f;
        public float RopeSlackFactor = 0.25f;
        public float HookPullForce = 3500f;
        public float WheelInOutFactor = 2f;
        public float InitialHookAttachmentPropulsionForceFactor = 5f;

        // SmartAim
        public float SmartAimRadius = 3f;
    }
}