using System;
using JetBrains.Annotations;
using Modules.Shared.Cooldown;
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
        protected IProjectileLauncher ProjectileLauncher;
        protected WeaponAnimator Animator;

        protected float FireCooldown = 0.7f;
        protected CooldownHandler FireCooldownHandler;

        private Coroutine _reloadRoutine;
        private Coroutine _shootingRoutine;

        protected GameObject Owner;
        
        [CanBeNull]
        protected Predicate<IWeapon> PreventFirePredicate;

        private void Awake()
        {
            FireCooldownHandler = gameObject.AddComponent<CooldownHandler>();
            _audio = GetComponent<WeaponAudioHandler>();
            ProjectileLauncher = GetComponent<IProjectileLauncher>();
            Ammo = gameObject.AddComponent<Ammo.Ammo>();
            Animator = GetComponent<WeaponAnimator>();

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
            if (PreventFirePredicate != null && PreventFirePredicate.Invoke(this))
            {
                return;
            }
            
            if (FireCooldownHandler.CooldownActive())
            {
                return;
            }

            if (Ammo.HasActiveContinuousReload())
            {
                Ammo.InterruptReload();
            }

            Ammo.ConsumeSingleWithActionOrElse(() =>
            {
                FireCooldownHandler.StartCooldown(FireCooldown);
                _shootingRoutine = StartCoroutine(Animator.Fire(FireCooldown));
                ProjectileLauncher.Launch();
                _audio.PlayFireWeapon();
            }, ReloadWeapon);
        }

        public void AlternateFire()
        {
            ProjectileLauncher.GetActiveProjectiles().ForEach(p => p.TriggerAlternateAction());
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

        public void SetOwner(GameObject owner)
        {
            Owner = owner;
        }

        public GameObject GetOwner() => Owner;
    }
}