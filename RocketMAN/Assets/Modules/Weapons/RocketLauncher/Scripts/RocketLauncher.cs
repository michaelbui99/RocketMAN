using Modules.Weapons.Common.Scripts;
using UnityEngine;

namespace Modules.Weapons.RocketLauncher.Scripts
{
    public class RocketLauncher : MonoBehaviour, IWeapon
    {
        public event IWeapon.WeaponEventTrigger ReloadStartedEvent;
        public event IWeapon.WeaponEventTrigger ReloadFinishedEvent;

        private Ammo _ammo;
        private WeaponAudioHandler _audio;
        private IProjectileLauncher _projectileLauncher;

        private const float FireCooldown = 0.3f;
        private CooldownHandler _cooldownHandler;

        private void Awake()
        {
            _cooldownHandler = gameObject.AddComponent<CooldownHandler>();
            _audio = GetComponent<WeaponAudioHandler>();
            _projectileLauncher = GetComponent<IProjectileLauncher>();
            _ammo = gameObject.AddComponent<Ammo>();

            // NOTE: (mibui 2023-04-20) Ammo is owned by RocketLauncher and they share lifetime. Should be fine to not unsubscribe
            _ammo.ReloadStartedEvent += () =>
            {
                if (_ammo.RemainingAmmoCount == 0)
                {
                    return;
                }

                ReloadStartedEvent?.Invoke();
                _audio.PlayReload();
            };

            _ammo.ReloadFinishedEvent += () => { ReloadFinishedEvent?.Invoke(); };
        }

        private void Start()
        {
            _ammo.ReloadBehaviour = ReloadBehaviour.Continuous;
            _ammo.TotalReloadUnits = 20;
            _ammo.AmmoPerReloadUnit = 1;
            _ammo.ClipSize = 5;
            _ammo.CurrentAmmoCount = _ammo.ClipSize;
            _ammo.RemainingAmmoCount = 20;
            _ammo.ReloadTime = 0.5f;
        }

        public void FireWeapon()
        {
            if (_cooldownHandler.CooldownActive())
            {
                return;
            }
            
            if (_ammo.HasActiveReload())
            {
                _ammo.InterruptReload();
            }

            _ammo.ConsumeSingleWithActionOrElse(() =>
            {
                _cooldownHandler.StartCooldown(FireCooldown);
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
            _ammo.Reload();
        }

        public float GetFireCooldown()
        {
            return FireCooldown;
        }

        public int GetCurrentAmmoCount() => _ammo.CurrentAmmoCount;
        public int GetRemainingAmmoCount() => _ammo.RemainingAmmoCount;
    }
}