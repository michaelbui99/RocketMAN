using System;
using UnityEngine;
using Utility;

namespace OdmGear.GrappleHooks
{
    public class HookCooldownHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GrappleHookSettings globalSettings;

        private bool _cooldownActive = false;
        private DateTime? _hookLaunchedTime;

        public bool CooldownActive() => _cooldownActive;

        private void Update()
        {
            if (_hookLaunchedTime is null)
            {
                return;
            }

            if (TimeUtil.GetTimeDifferenceInSeconds(_hookLaunchedTime.Value, DateTime.UtcNow) >=
                globalSettings.HookCooldownInSeconds)
            {
                _cooldownActive = false;
            }
        }

        public void StartCoolDown()
        {
            _cooldownActive = true;
            _hookLaunchedTime = DateTime.UtcNow;
        }
    }
}