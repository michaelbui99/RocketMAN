using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Modules.Events;
using Modules.Weapons.Common.Scripts.Ammo;
using Modules.Weapons.Common.Scripts.Weapon;
using UnityEngine;
using Utility;

namespace Modules.Weapons.WeaponManager.Scripts
{
    public class WeaponManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameObject owner;

        [SerializeField]
        private GameEvent weaponStateEvent;

        [SerializeField]
        private GameObject weaponHolder;

        [SerializeField]
        private WeaponModuleFactory moduleFactory;

        private readonly Dictionary<string, AmmoState> _weaponToAmmoStateMap = new();

        [CanBeNull]
        private WeaponModule _currentWeaponModule = null;

        private IWeaponInput _weaponInput;

        private readonly CurrentWeapon _currentWeapon = new();
        private GameEventObserver _ammoPickupObserver;

        private IPausedGameObserver _pausedGameObserver;

        private void Awake()
        {
            _pausedGameObserver = GetComponentInChildren<IPausedGameObserver>();
            _ammoPickupObserver = GetComponent<GameEventObserver>();
            _weaponInput = GetComponent<IWeaponInput>();
            _currentWeaponModule = moduleFactory.GetDefault();

            SwitchWeapon(_currentWeaponModule switch
            {
                null => moduleFactory.GetDefault().InternalWeaponName,
                WeaponModule module => module.InternalWeaponName
            });
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
            if (_pausedGameObserver.GameIsPaused())
            {
                return;
            }

            if (_currentWeapon.instance is null)
            {
                return;
            }

            var weaponTransform = _currentWeapon.instance.transform;
            var weaponHolderTransform = weaponHolder.transform;

            weaponTransform.position = weaponHolderTransform.position;

            weaponTransform.localPosition += _currentWeaponModule switch
            {
                null => throw new ArgumentException("No Module"),
                WeaponModule module => module.WeaponPositionOffset
            };

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
            if (_pausedGameObserver.GameIsPaused())
            {
                return;
            }

            _currentWeapon.WeaponComponent.FireWeapon();
            EmitFireWeaponEvent();
        }

        private void CurrentWeaponAlternateFire()
        {
            if (_pausedGameObserver.GameIsPaused())
            {
                return;
            }

            _currentWeapon.WeaponComponent.AlternateFire();
            EmitFireWeaponEvent();
        }

        private void ReloadCurrentWeapon()
        {
            if (_pausedGameObserver.GameIsPaused())
            {
                return;
            }

            _currentWeapon.WeaponComponent.ReloadWeapon();
        }

        private void InstantiateModule(WeaponModule module)
        {
            _currentWeaponModule = module;

            var weaponInstance = Instantiate(
                _currentWeaponModule switch
                {
                    null => throw new ArgumentException("No Weapon Module"),
                    WeaponModule wm => wm.WeaponPrefab
                },
                weaponHolder.transform, false);

            var weaponComponent = weaponInstance.GetComponent<IWeapon>();
            weaponComponent.SetOwner(owner);

            _currentWeapon.instance = weaponInstance;
            _currentWeapon.WeaponComponent = weaponComponent;

            ConfigureAmmoForModule(module);
        }

        private void ConfigureAmmoForModule(WeaponModule module)
        {
            AmmoSettings ammoSettings = new()
            {
                ClipSize = module.ClipSize,
                TotalReloadUnits = module.TotalReloadUnits,
                AmmoPerReloadUnit = module.AmmoPerReloadUnit,
                ReloadBehaviour = module.ReloadBehaviour,
                ReloadTime = module.ReloadTime,
                UnlimitedAmmo = module.UnlimitedAmmo
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
                WeaponName = _currentWeaponModule switch
                {
                    null => throw new ArgumentException("No Weapon Module"),
                    WeaponModule module => module.DisplayName
                },
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

            _currentWeapon.WeaponComponent.RestoreAmmo(reloadUnits.Cast<int>());
            EmitReloadFinishedStateChange();
        }
    }
}