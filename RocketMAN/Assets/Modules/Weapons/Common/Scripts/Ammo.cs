using System;
using System.Collections;
using UI;
using UnityEngine;

namespace Modules.Weapons.Common.Scripts
{
    public class Ammo : MonoBehaviour
    {
        public delegate void ReloadEventTrigger();

        public event ReloadEventTrigger ReloadStartedEvent;
        public event ReloadEventTrigger ReloadFinishedEvent;
        public event ReloadEventTrigger ReloadBlockedEvent;
        public int ClipSize { get; set; }
        public int CurrentAmmoCount { get; set; }
        public int RemainingAmmoCount { get; set; }
        public int AmmoPerReloadUnit { get; set; }
        public int TotalReloadUnits { get; set; }
        public float ReloadTime { get; set; }
        public ReloadBehaviour ReloadBehaviour { get; set; }

        private bool _reloading = false;
        private CooldownHandler _cooldownHandler;

        private void Awake()
        {
            _cooldownHandler = gameObject.AddComponent<CooldownHandler>();
            _cooldownHandler.CooldownFinishedEvent += () => ReloadFinishedEvent?.Invoke();
            _cooldownHandler.CooldownStartedEvent += () => ReloadStartedEvent?.Invoke();
        }

        private void OnDestroy()
        {
            Destroy(_cooldownHandler);
        }

        private void Update()
        {
            if (!_reloading || _cooldownHandler.CooldownActive())
            {
                return;
            }

            if (CurrentAmmoCount == ClipSize)
            {
                InterruptReload();
                return;
            }

            if (AmmoPerReloadUnit > RemainingAmmoCount)
            {
                _cooldownHandler.StartCooldown(ReloadTime);
                CurrentAmmoCount += RemainingAmmoCount;
                RemainingAmmoCount = 0;
                InterruptReload();
                return;
            }

            _cooldownHandler.StartCooldown(ReloadTime);
            CurrentAmmoCount += AmmoPerReloadUnit;
            RemainingAmmoCount -= AmmoPerReloadUnit;
        }

        public bool ConsumeSingle()
        {
            if (ShouldReload() || _reloading)
            {
                return false;
            }

            CurrentAmmoCount--;
            return true;
        }

        public void ConsumeSingleWithActionOrElse(Action onConsume, Action orElse)
        {
            if (!ConsumeSingleWithAction(onConsume))
            {
                orElse.Invoke();
            }
        }

        public bool ConsumeSingleWithAction(Action onConsume)
        {
            if (ShouldReload() || _reloading)
            {
                return false;
            }

            CurrentAmmoCount--;
            onConsume.Invoke();
            return true;
        }

        public void Reload()
        {
            if (_cooldownHandler.CooldownActive())
            {
                ReloadBlockedEvent?.Invoke();
            }

            if (ReloadBehaviour == ReloadBehaviour.Discrete)
            {
                if (GetMissingAmmo() > RemainingAmmoCount)
                {
                    _cooldownHandler.StartCooldown(ReloadTime);
                    CurrentAmmoCount += RemainingAmmoCount;
                    RemainingAmmoCount = 0;
                    return;
                }

                _cooldownHandler.StartCooldown(ReloadTime);
                var missing = GetMissingAmmo();
                CurrentAmmoCount += missing;
                RemainingAmmoCount -= missing;
            }

            if (ReloadBehaviour == ReloadBehaviour.Continuous)
            {
                _reloading = true;
            }
        }

        public void InterruptReload()
        {
            _reloading = false;
            _cooldownHandler.ForceResetCooldown();
        }

        public bool ShouldReload() => CurrentAmmoCount == 0;
        public bool HasActiveReload() => _reloading;

        private int GetMissingAmmo() => ClipSize - CurrentAmmoCount;
    }

    public enum ReloadBehaviour
    {
        // Either reloaded fully or not, e.g. like a AK47
        Discrete,

        // Reloads continuously one ammo until fully reloaded, e.g. like Rocket Launcher from TF2
        Continuous,
    }
}