using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Modules.Events;
using Modules.Shared.Cooldown;
using Modules.Weapons.Common.Scripts.Ammo;
using Modules.Weapons.Common.Scripts.Weapon;
using Modules.Weapons.WeaponManager.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

namespace Modules.Odm.Scripts
{
    public class OdmHooks : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameObject owner;

        [SerializeField]
        private GameObject leftGrapplingGun;

        [SerializeField]
        private GameObject rightGrapplingGun;

        [SerializeField]
        private WeaponModule grapplingGunModule;

        [SerializeField]
        private GameEvent weaponStateEvent;

        private IOdmInput _odmInput;

        private IWeapon _leftGrapplingGunWeapon;
        private IWeapon _rightGrapplingGunWeapon;

        private void Awake()
        {
            _odmInput = GetComponent<IOdmInput>();
            _leftGrapplingGunWeapon = leftGrapplingGun.GetComponent<IWeapon>();
            _rightGrapplingGunWeapon = rightGrapplingGun.GetComponent<IWeapon>();
            _odmInput.OnLeftHookFire += LaunchLeftHook;
            _odmInput.OnRightHookFire += LaunchRightHook;
            _odmInput.OnLeftHookRelease += _leftGrapplingGunWeapon.AlternateFire;
            _odmInput.OnRightHookRelease += _rightGrapplingGunWeapon.AlternateFire;
        }

        private void Start()
        {
            SetupGrapplingGun(_leftGrapplingGunWeapon);
            SetupGrapplingGun(_rightGrapplingGunWeapon);
        }

        private void OnDestroy()
        {
            _odmInput.OnLeftHookFire -= LaunchLeftHook;
            _odmInput.OnRightHookFire -= LaunchRightHook;
            _odmInput.OnLeftHookRelease -= _leftGrapplingGunWeapon.AlternateFire;
            _odmInput.OnRightHookRelease -= _rightGrapplingGunWeapon.AlternateFire;
        }

        private void LateUpdate()
        {
            gameObject.transform.forward = owner.transform.forward;
        }

        private void LaunchLeftHook()
        {
            try
            {
                gameObject.transform.forward = owner.transform.forward;
                _leftGrapplingGunWeapon.FireWeapon();
                RaiseFireHookEvent();
            }
            catch (Exception e)
            {
                // NOTE: (mibui 2023-05-08) Retry again after configuring ammo again just in case instantiation failed
                SetupGrapplingGun(_leftGrapplingGunWeapon);
                gameObject.transform.forward = owner.transform.forward;
                _leftGrapplingGunWeapon.FireWeapon();
                RaiseFireHookEvent();
            }
        }

        private void LaunchRightHook()
        {
            try
            {
                gameObject.transform.forward = owner.transform.forward;
                _rightGrapplingGunWeapon.FireWeapon();
                RaiseFireHookEvent();
            }
            catch (Exception e)
            {
                // NOTE: (mibui 2023-05-08) Retry again after configuring ammo again just in case instantiation failed
                SetupGrapplingGun(_rightGrapplingGunWeapon);
                gameObject.transform.forward = owner.transform.forward;
                _rightGrapplingGunWeapon.FireWeapon();
                RaiseFireHookEvent();
            }
        }

        private void SetupGrapplingGun(IWeapon grapplingGun)
        {
            grapplingGun.SetOwner(owner);

            AmmoSettings ammoSettings = new()
            {
                ClipSize = grapplingGunModule.ClipSize,
                UnlimitedAmmo = grapplingGunModule.UnlimitedAmmo,
                ReloadTime = grapplingGunModule.ReloadTime,
                TotalReloadUnits = grapplingGunModule.TotalReloadUnits,
                AmmoPerReloadUnit = grapplingGunModule.AmmoPerReloadUnit,
                ReloadBehaviour = grapplingGunModule.ReloadBehaviour
            };

            AmmoState state = new()
            {
                CurrentAmmoCount = grapplingGunModule.ClipSize,
                RemainingAmmoCount = grapplingGunModule.TotalReloadUnits
            };

            grapplingGun.SetAmmoSettings(ammoSettings);
            grapplingGun.SetAmmoState(state);
        }

        private void RaiseFireHookEvent()
        {
            weaponStateEvent.Raise(new WeaponStateEvent()
            {
                EventType = WeaponStateEventType.FireWeapon,
                ReloadTime = grapplingGunModule.ReloadTime,
                CurrentAmmo = 1,
                RemainingAmmo = 1,
                FireCooldown = _leftGrapplingGunWeapon.GetFireCooldown(),
                WeaponName = grapplingGunModule.InternalWeaponName
            });
        }
    }
}