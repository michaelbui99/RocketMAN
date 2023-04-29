using Modules.Weapons.Common.Scripts.Ammo;
using Modules.Weapons.Common.Scripts.Launchers;
using UnityEngine;

namespace Modules.Weapons.Common.Scripts.Weapon
{
    public class WeaponBase : MonoBehaviour, IWeapon
    {
        public event IWeapon.WeaponEventTrigger ReloadStartedEvent;
        public event IWeapon.WeaponEventTrigger ReloadFinishedEvent;
        protected Ammo.Ammo Ammo;
        private WeaponAudioHandler _audio;
        private IProjectileLauncher _projectileLauncher;
        private WeaponAnimator _animator;

        protected float FireCooldown = 0.7f;
        private CooldownHandler _fireCooldownHandler;

        private Coroutine _reloadRoutine;
        private Coroutine _shootingRoutine;

        private void Awake()
        {
            _fireCooldownHandler = gameObject.AddComponent<CooldownHandler>();
            _audio = GetComponent<WeaponAudioHandler>();
            _projectileLauncher = GetComponent<IProjectileLauncher>();
            Ammo = gameObject.AddComponent<Ammo.Ammo>();
            _animator = GetComponent<WeaponAnimator>();

            // NOTE: (mibui 2023-04-20) Ammo is owned by the weapon and they share lifetime. Should be fine to not unsubscribe
            Ammo.ReloadStartedEvent += () =>
            {
                if (Ammo.RemainingAmmoCount == 0)
                {
                    return;
                }

                ReloadStartedEvent?.Invoke();
                _audio.PlayReload();
            };

            Ammo.AmmoDepletedEvent += ReloadWeapon;
        }

        public void FireWeapon()
        {
            if (_fireCooldownHandler.CooldownActive())
            {
                return;
            }

            if (Ammo.HasActiveContinuousReload())
            {
                Ammo.InterruptReload();
            }

            Ammo.ConsumeSingleWithActionOrElse(() =>
            {
                _fireCooldownHandler.StartCooldown(FireCooldown);
                _shootingRoutine = StartCoroutine(_animator.Fire(FireCooldown));
                _projectileLauncher.Launch();
                _audio.PlayFireWeapon();
            }, ReloadWeapon);
        }

        public void AlternateFire()
        {
            _projectileLauncher.GetActiveProjectiles().ForEach(p => p.TriggerAlternateAction());
        }

        public void ReloadWeapon()
        {
            Ammo.Reload();
        }

        public float GetFireCooldown()
        {
            return FireCooldown;
        }

        public int GetCurrentAmmoCount() => Ammo.CurrentAmmoCount;
        public int GetRemainingAmmoCount() => Ammo.RemainingAmmoCount;

        public void SetAmmoState(AmmoState ammoState)
        {
            Ammo.AmmoState = ammoState;
        }

        public void SetAmmoSettings(AmmoSettings ammoSettings)
        {
            Ammo.AmmoSettings = ammoSettings;
        }

        public void RestoreAmmo(int reloadUnits)
        {
            Ammo.RestoreAmmo(reloadUnits);
        }
    }
}