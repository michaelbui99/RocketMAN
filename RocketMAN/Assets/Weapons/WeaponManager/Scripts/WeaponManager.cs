using System;
using UnityEngine;
using Utility;
using Weapons.Common;
using Weapons.Common.Scripts;

namespace Weapons.WeaponManager.Scripts
{
    public class WeaponManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameObject weaponHolder;

        private readonly IWeaponModuleFactory _moduleFactory = new WeaponModuleFactory();
        private Optional<IWeaponModule> _currentWeaponModule = Optional<IWeaponModule>.Empty();
        private IWeaponInput _weaponInput;

        private CurrentWeapon _currentWeapon = new();

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
        }

        private void OnDestroy()
        {
            _weaponInput.OnFireWeapon -= FireCurrentWeapon;
            _weaponInput.OnSwitchWeapon -= SwitchWeapon;
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
            weaponTransform.forward = weaponHolderTransform.forward;
        }

        private void SwitchWeapon(string weapon)
        {
            Destroy(_currentWeapon.instance);
            _currentWeaponModule = null;
            
            var module = _moduleFactory.Create(weapon);
            _currentWeaponModule = Optional<IWeaponModule>.From(module);

            var weaponInstance =
                Instantiate(_currentWeaponModule
                    .GetOrThrow(() => new ArgumentException("No weapon module"))
                    .WeaponPrefab, weaponHolder.transform, false);

            var weaponComponent = weaponInstance.GetComponent<IWeapon>();

            _currentWeapon.instance = weaponInstance;
            _currentWeapon.WeaponComponent = weaponComponent;
        }

        private void FireCurrentWeapon()
        {
            _currentWeapon.WeaponComponent.FireWeapon();
        }
    }
}