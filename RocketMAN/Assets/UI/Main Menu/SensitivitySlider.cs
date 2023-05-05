using System;
using Modules.GameSettings.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu
{
    public class SensitivitySlider : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private TMP_Text value;
        [SerializeField]
        private Slider slider;

        private GameSettingsIO _gameSettingsIO;

        private void Awake()
        {
            _gameSettingsIO = GetComponent<GameSettingsIO>();
            slider.value = _gameSettingsIO.PlayerSensitivitySettings.VerticalMouseSensitivity;
        }

        void Update()
        {
            value.text = $"{slider.value.ToString("0.00")}";
            _gameSettingsIO.PlayerSensitivitySettings.VerticalMouseSensitivity = slider.value;
        }
    }
}
