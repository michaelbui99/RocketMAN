using System;
using Modules.Events;
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

        private GameEventObserver _weaponStateObserver;

        private void Awake()
        {
            _weaponStateObserver = GetComponent<GameEventObserver>();
        }

        void Start()
        {
            _weaponStateObserver.RegisterCallback(OnWeaponStateChange);
        }

        private void OnDestroy()
        {
            _weaponStateObserver.UnregisterCallback(OnWeaponStateChange);
        }

        private void OnWeaponStateChange(object state)
        {
            WeaponStateEvent stateData = (WeaponStateEvent) state;
            ammoText.text = $"{stateData.CurrentAmmo}/{stateData.RemainingAmmo}";
        }
    }
}