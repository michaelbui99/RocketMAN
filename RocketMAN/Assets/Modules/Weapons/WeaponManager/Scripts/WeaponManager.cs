using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Events;
using Modules.Weapons.Common.Scripts;
using Modules.Weapons.Common.Scripts.Ammo;
using Modules.Weapons.Common.Scripts.Weapon;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

namespace Modules.Weapons.WeaponManager.Scripts
{
    public class WeaponManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameEvent weaponStateEvent;

        [SerializeField]
        private GameObject weaponHolder;

        [SerializeField]
        private WeaponModuleFactory moduleFactory;

        private readonly Dictionary<string, AmmoState> _weaponToAmmoStateMap = new();

        private Optional<WeaponModule> _currentWeaponModule = Optional<WeaponModule>.Empty();
        private IWeaponInput _weaponInput;

        private readonly CurrentWeapon _currentWeapon = new();
        private GameEventObserver _ammoPickupObserver;

        private void Awake()
        {
            _ammoPickupObserver = GetComponent<GameEventObserver>();
            _weaponInput = GetComponent<IWeaponInput>();
            _currentWeaponModule = Optional<WeaponModule>.From(moduleFactory.GetDefault());

            SwitchWeapon(_currentWeaponModule
                .GetOrElseGet(() => moduleFactory.GetDefault())
                .InternalWeaponName
            );
            _ammoPickupObserver.RegisterCallback(RestoreAmmo);
        }

        private void Start()
        {
            _weaponInput.OnFireWeapon += FireCurrentWeapon;
            _weaponInput.OnSwitchWeapon += SwitchWeapon;
            _weaponInput.OnReloadWeapon += ReloadCurrentWeapon;
            _weaponInput.OnAlternateFire += CurrentWeaponAlternateFire;
            EmitReloadFinishedStateChange();
        }

        private void OnDestroy()
        {
            _weaponInput.OnFireWeapon -= FireCurrentWeapon;
            _weaponInput.OnSwitchWeapon -= SwitchWeapon;
            _weaponInput.OnReloadWeapon -= ReloadCurrentWeapon;
            _weaponInput.OnAlternateFire -= CurrentWeaponAlternateFire;
        }

        private void LateUpdate()
        {
            if (_currentWeapon.instance is null)
            {
                return;
            }

            var weaponTransform = _currentWeapon.instance.transform;
            var weaponHolderTransform = weaponHolder.transform;

            weaponTransform.position = weaponHolderTransform.position;

            weaponTransform.localPosition += _currentWeaponModule
                .GetOrThrow(() => new ArgumentException("No Module"))
                .WeaponPositionOffset;

            weaponTransform.forward = weaponHolderTransform.forward.normalized;
            weaponStateEvent.Raise(CreateEvent(WeaponStateEventType.Status));
        }

        private void SwitchWeapon(string weapon)
        {
            if (_currentWeapon.instance != null && _currentWeapon.WeaponComponent != null)
            {
                _currentWeapon.WeaponComponent.ReloadFinishedEvent -= EmitReloadFinishedStateChange;
                _currentWeapon.WeaponComponent.ReloadFinishedEvent -= EmitReloadStartedStateChange;
                Destroy(_currentWeapon.instance);
            }

            _currentWeaponModule = null;

            var module = moduleFactory.Create(weapon);
            InstantiateModule(module);
            // NOTE: (mibui 2023-04-21) Emit when reload finished to ensure that it is updated state that gets emitted
            //                          as it takes some time to reload.
            _currentWeapon.WeaponComponent!.ReloadFinishedEvent += EmitReloadFinishedStateChange;
            _currentWeapon.WeaponComponent!.ReloadStartedEvent += EmitReloadStartedStateChange;
        }

        private void FireCurrentWeapon()
        {
            _currentWeapon.WeaponComponent.FireWeapon();
            EmitFireWeaponEvent();
        }

        private void CurrentWeaponAlternateFire()
        {
            _currentWeapon.WeaponComponent.AlternateFire();
            EmitFireWeaponEvent();
        }

        private void ReloadCurrentWeapon()
        {
            _currentWeapon.WeaponComponent.ReloadWeapon();
        }

        private void InstantiateModule(WeaponModule module)
        {
            _currentWeaponModule = Optional<WeaponModule>.From(module);

            var weaponInstance =
                Instantiate(
                    _currentWeaponModule
                        .GetOrThrow(() => new ArgumentException("No weapon module")).WeaponPrefab,
                    weaponHolder.transform, false
                );

            var weaponComponent = weaponInstance.GetComponent<IWeapon>();

            _currentWeapon.instance = weaponInstance;
            _currentWeapon.WeaponComponent = weaponComponent;

            AmmoSettings ammoSettings = new()
            {
                ClipSize = module.ClipSize,
                TotalReloadUnits = module.TotalReloadUnits,
                AmmoPerReloadUnit = module.AmmoPerReloadUnit,
                ReloadBehaviour = module.ReloadBehaviour,
                ReloadTime = module.ReloadTime
            };

            if (_weaponToAmmoStateMap.TryGetValue(module.InternalWeaponName, out AmmoState ammoState))
            {
                _currentWeapon.WeaponComponent.SetAmmoState(ammoState);
                _currentWeapon.WeaponComponent.SetAmmoSettings(ammoSettings);
            }
            else
            {
                ammoState = new AmmoState
                {
                    CurrentAmmoCount = ammoSettings.ClipSize,
                    RemainingAmmoCount = ammoSettings.TotalReloadUnits * ammoSettings.AmmoPerReloadUnit
                };

                _weaponToAmmoStateMap.Add(module.InternalWeaponName, ammoState);
                _currentWeapon.WeaponComponent.SetAmmoState(_weaponToAmmoStateMap[module.InternalWeaponName]);
                _currentWeapon.WeaponComponent.SetAmmoSettings(ammoSettings);
            }
        }

        private WeaponStateEvent CreateEvent(WeaponStateEventType type)
        {
            return new WeaponStateEvent()
            {
                WeaponName = _currentWeaponModule
                    .GetOrThrow(() => new ArgumentException("No module"))
                    .DisplayName,
                CurrentAmmo = _currentWeapon.WeaponComponent.GetCurrentAmmoCount(),
                RemainingAmmo = _currentWeapon.WeaponComponent.GetRemainingAmmoCount(),
                EventType = type
            };
        }

        private void EmitReloadStartedStateChange()
        {
            weaponStateEvent.Raise(CreateEvent(WeaponStateEventType.ReloadStarted));
        }

        private void EmitReloadFinishedStateChange()
        {
            weaponStateEvent.Raise(CreateEvent(WeaponStateEventType.ReloadFinished));
        }

        private void EmitFireWeaponEvent()
        {
            weaponStateEvent.Raise(CreateEvent(WeaponStateEventType.FireWeapon));
        }

        public void RestoreAmmo(object reloadUnits)
        {
            if (_currentWeapon.instance is null || _currentWeapon.WeaponComponent is null)
            {
                return;
            }

            _currentWeapon.WeaponComponent.RestoreAmmo((int) reloadUnits);
            EmitReloadFinishedStateChange();
        }
    }
}