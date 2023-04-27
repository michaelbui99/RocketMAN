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
        public event ReloadEventTrigger AmmoDepletedEvent;
        public int ClipSize { get; set; }
        public AmmoState AmmoState { get; set; }
        public int CurrentAmmoCount() => AmmoState.CurrentAmmoCount;
        public int RemainingAmmoCount() => AmmoState.RemainingAmmoCount;
        public int AmmoPerReloadUnit { get; set; }
        public int TotalReloadUnits { get; set; }
        public float ReloadTime { get; set; }
        public ReloadBehaviour ReloadBehaviour { get; set; }

        private bool _reloadingContinously = false;
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
            if (CurrentAmmoCount() == 0)
            {
                AmmoDepletedEvent?.Invoke();
            }

            if (!_reloadingContinously || _cooldownHandler.CooldownActive())
            {
                return;
            }

            if (CurrentAmmoCount() == ClipSize)
            {
                InterruptReload();
                return;
            }

            if (AmmoPerReloadUnit > RemainingAmmoCount())
            {
                _cooldownHandler.StartCooldown(ReloadTime);
                AmmoState.CurrentAmmoCount += RemainingAmmoCount();
                AmmoState.RemainingAmmoCount = 0;
                InterruptReload();
                return;
            }

            _cooldownHandler.StartCooldown(ReloadTime);
            AmmoState.CurrentAmmoCount += AmmoPerReloadUnit;
            AmmoState.RemainingAmmoCount -= AmmoPerReloadUnit;
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
            if (ShouldReload() || _reloadingContinously || _cooldownHandler.CooldownActive())
            {
                return false;
            }

            AmmoState.CurrentAmmoCount -= 1;
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
                if (GetMissingAmmo() > RemainingAmmoCount())
                {
                    _cooldownHandler.StartCooldown(ReloadTime);
                    AmmoState.CurrentAmmoCount += RemainingAmmoCount();
                    AmmoState.RemainingAmmoCount = 0;
                    return;
                }

                _cooldownHandler.StartCooldown(ReloadTime);
                var missing = GetMissingAmmo();
                AmmoState.CurrentAmmoCount += missing;
                AmmoState.RemainingAmmoCount -= missing;
            }

            if (ReloadBehaviour == ReloadBehaviour.Continuous)
            {
                _reloadingContinously = true;
            }
        }

        public void InterruptReload()
        {
            _reloadingContinously = false;
            _cooldownHandler.ForceResetCooldown();
        }

        public void RestoreAmmo(int reloadUnits)
        {
            var ammoToRestore = reloadUnits * AmmoPerReloadUnit;
            AmmoState.RemainingAmmoCount += ammoToRestore;

            var remainingAmmoInReloadUnits = RemainingAmmoCount() / AmmoPerReloadUnit;
            if (remainingAmmoInReloadUnits > TotalReloadUnits)
            {
                AmmoState.RemainingAmmoCount = TotalReloadUnits * AmmoPerReloadUnit;
            }
        }

        public bool ShouldReload() => CurrentAmmoCount() == 0;
        public bool HasActiveContinuousReload() => _reloadingContinously;

        private int GetMissingAmmo() => ClipSize - CurrentAmmoCount();
    }

    public enum ReloadBehaviour
    {
        // Either reloaded fully or not, e.g. like an AK47
        Discrete,

        // Reloads continuously one ammo until fully reloaded, e.g. like Rocket Launcher from TF2
        Continuous,
    }
}