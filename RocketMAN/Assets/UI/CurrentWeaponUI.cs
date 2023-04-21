using System;
using System.Linq;
using Modules.Weapons.WeaponManager.Scripts;
using TMPro;
using UnityEngine;
using StringBuilder = System.Text.StringBuilder;

namespace UI
{
    public class CurrentWeaponUI : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private WeaponManager weaponManager;

        [SerializeField]
        private TMP_Text currentWeapon;

        private void Awake()
        {
            weaponManager.WeaponStateChangeEvent += OnWeaponStateChange;
        }

        private void OnDestroy()
        {
            weaponManager.WeaponStateChangeEvent -= OnWeaponStateChange;
        }

        private void OnWeaponStateChange(WeaponStateEvent state)
        {
            var nameParts = state.WeaponName.Split("_");
            var processedParts = nameParts
                .Select(p => p.ToLower())
                .Select(p => new StringBuilder(p))
                .Select(p =>string.Concat(p[0].ToString().ToUpper(), p.ToString().AsSpan(1).ToString()))
                .ToList();

            var s = new StringBuilder();
            
            for (int i = 0; i < processedParts.Count; i++)
            {
                s.Append(processedParts[i]);
                
                if (i != processedParts.Count - 1)
                {
                    s.Append(" ");
                }
            }

            currentWeapon.text = s.ToString();
        }
    }
}