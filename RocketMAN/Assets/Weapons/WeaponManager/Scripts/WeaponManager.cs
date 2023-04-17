using System;
using UnityEngine;
using Utility;

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

        private GameObject _currentWeaponInstance;

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
            if (_currentWeaponInstance is null)
            {
                return;
            }

            var weaponTransform = _currentWeaponInstance.transform;
            var weaponHolderTransform = weaponHolder.transform;
            weaponTransform.position = weaponHolderTransform.position;
            weaponTransform.forward = weaponHolderTransform.forward;
        }

        private void SwitchWeapon(string weapon)
        {
            Destroy(_currentWeaponInstance);
            var module = _moduleFactory.Create(weapon);
            _currentWeaponModule = Optional<IWeaponModule>.From(module);

            _currentWeaponInstance =
                Instantiate(_currentWeaponModule
                    .GetOrThrow(() => new ArgumentException("No weapon module"))
                    .WeaponPrefab, weaponHolder.transform, false);
        }

        private void FireCurrentWeapon()
        {
            _currentWeaponModule.Bind(weapon =>
            {
                weapon.FireWeapon();
                return weapon;
            });
        }
    }
}