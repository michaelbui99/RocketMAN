using System;
using Modules.Weapons.Common.Scripts;
using TMPro.EditorUtilities;
using UnityEngine;
using Utility;

namespace Modules.Weapons.WeaponManager.Scripts
{
    public class WeaponManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameObject weaponHolder;

        [SerializeField]
        private WeaponModuleFactory moduleFactory;

        private Optional<WeaponModule> _currentWeaponModule = Optional<WeaponModule>.Empty();
        private IWeaponInput _weaponInput;

        private readonly CurrentWeapon _currentWeapon = new();

        public delegate void OnWeaponStateChange(WeaponStateEvent state);

        public OnWeaponStateChange WeaponStateChangeEvent;

        private void Awake()
        {
            _weaponInput = GetComponent<IWeaponInput>();
            _currentWeaponModule = Optional<WeaponModule>.From(moduleFactory.GetDefault());

            SwitchWeapon(_currentWeaponModule
                .GetOrElseGet(() => moduleFactory.GetDefault())
                .InternalWeaponName
            );
        }

        private void Start()
        {
            _weaponInput.OnFireWeapon += FireCurrentWeapon;
            _weaponInput.OnSwitchWeapon += SwitchWeapon;
            _weaponInput.OnReloadWeapon += ReloadCurrentWeapon;
            EmitReloadFinishedStateChange();
        }

        private void OnDestroy()
        {
            _weaponInput.OnFireWeapon -= FireCurrentWeapon;
            _weaponInput.OnSwitchWeapon -= SwitchWeapon;
            _weaponInput.OnReloadWeapon -= ReloadCurrentWeapon;
        }

        private void LateUpdate()
        {
            if (_currentWeapon.instance is null)
            {
                return;
            }

            var weaponTransform = _currentWeapon.instance.transform;
            var weaponHolderTransform = weaponHolder.transform;

            weaponTransform.position = weaponHolderTransform.position + _currentWeaponModule
                .GetOrThrow(() => new ArgumentException("No Module"))
                .WeaponPositionOffset;

            weaponTransform.forward = weaponHolderTransform.forward.normalized;
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
            WeaponStateChangeEvent?.Invoke(CreateEvent(WeaponStateEventType.ReloadStarted));
        }

        private void EmitReloadFinishedStateChange()
        {
            WeaponStateChangeEvent?.Invoke(CreateEvent(WeaponStateEventType.ReloadFinished));
        }

        private void EmitFireWeaponEvent()
        {
            WeaponStateChangeEvent?.Invoke(CreateEvent(WeaponStateEventType.FireWeapon));
        }
    }
}