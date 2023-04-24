using System;
using Modules.Weapons.WeaponManager.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class AmmoUI : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private WeaponManager weaponManager;

        [SerializeField]
        private TMP_Text ammoText;

        void Start()
        {
            weaponManager.WeaponStateChangeEvent += OnWeaponStateChange;
        }

        private void OnDestroy()
        {
            weaponManager.WeaponStateChangeEvent -= OnWeaponStateChange;
        }

        private void OnWeaponStateChange(WeaponStateEvent state)
        {
            ammoText.text = $"{state.CurrentAmmo}/{state.RemainingAmmo}";
        }
    }
}