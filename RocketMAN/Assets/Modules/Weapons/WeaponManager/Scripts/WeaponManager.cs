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

        private readonly IWeaponModuleFactory _moduleFactory = new WeaponModuleFactory();
        private Optional<IWeaponModule> _currentWeaponModule = Optional<IWeaponModule>.Empty();
        private IWeaponInput _weaponInput;

        private readonly CurrentWeapon _currentWeapon = new();

        public delegate void OnWeaponStateChange(WeaponStateEvent state);

        public OnWeaponStateChange WeaponStateChangeEvent;

        private void Awake()
        {
            _weaponInput = GetComponent<IWeaponInput>();
            _currentWeaponModule = Optional<IWeaponModule>.From(_moduleFactory.GetDefault());

            SwitchWeapon(_currentWeaponModule
                .GetOrElseGet(() => _moduleFactory.GetDefault())
                .GetWeaponName()
            );
        }

        private void Start()
        {
            _weaponInput.OnFireWeapon += FireCurrentWeapon;
            _weaponInput.OnSwitchWeapon += SwitchWeapon;
            _weaponInput.OnReloadWeapon += ReloadCurrentWeapon;
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
                .GetPositionOffSetVector()
                .GetValueOrDefault(Vector3.zero);

            weaponTransform.forward = weaponHolderTransform.forward;
        }

        private void SwitchWeapon(string weapon)
        {
            if (_currentWeapon.instance != null && _currentWeapon.WeaponComponent != null)
            {
                _currentWeapon.WeaponComponent.ReloadFinishedEvent -= EmitStateChange;
                Destroy(_currentWeapon.instance);
            }

            _currentWeaponModule = null;

            var module = _moduleFactory.Create(weapon);
            InstantiateModule(module);
            // NOTE: (mibui 2023-04-21) Emit when reload finished to ensure that it is updated state that gets emitted
            //                          as it takes some time to reload.
            _currentWeapon.WeaponComponent!.ReloadFinishedEvent += EmitStateChange;
        }

        private void FireCurrentWeapon()
        {
            _currentWeapon.WeaponComponent.FireWeapon();
            EmitStateChange();
        }

        private void ReloadCurrentWeapon()
        {
            _currentWeapon.WeaponComponent.ReloadWeapon();
        }

        private void InstantiateModule(IWeaponModule module)
        {
            _currentWeaponModule = Optional<IWeaponModule>.From(module);

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

        private void EmitStateChange()
        {
            WeaponStateChangeEvent?.Invoke(new WeaponStateEvent()
            {
                WeaponName = _currentWeaponModule
                    .GetOrThrow(() => new ArgumentException("No module"))
                    .GetWeaponName(),
                CurrentAmmo = _currentWeapon.WeaponComponent.GetCurrentAmmoCount(),
                RemainingAmmo = _currentWeapon.WeaponComponent.GetRemainingAmmoCount()
            });
        }
    }
}