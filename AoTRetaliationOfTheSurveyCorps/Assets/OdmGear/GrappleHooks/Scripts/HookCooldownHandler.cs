using System;
using UnityEngine;
using Utility;

namespace OdmGear.GrappleHooks.Scripts
{
    public class HookCooldownHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GrappleHookSettings globalSettings;

        private float _cooldown = 0f;
        private bool _cooldownActive = false;
        private DateTime? _hookLaunchedTime;

        public bool CooldownActive() => _cooldownActive;

        private void Update()
        {
            if (!_hookLaunchedTime.HasValue)
            {
                return;
            }

            if (TimeUtil.GetTimeDifferenceInSeconds(_hookLaunchedTime.Value, DateTime.UtcNow) >=
                _cooldown)
            {
                _cooldownActive = false;
            }
        }

        public void StartCooldown(float? cooldown)
        {
            if (cooldown.HasValue)
            {
                _cooldown = cooldown.Value;
            }
            else
            {
                _cooldown = globalSettings.MaxHookCooldownInSeconds;
            }

            _cooldownActive = true;
            _hookLaunchedTime = DateTime.UtcNow;
        }
    }
}