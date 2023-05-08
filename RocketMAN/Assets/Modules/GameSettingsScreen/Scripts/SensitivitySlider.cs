using System;
using Modules.Events;
using Modules.GameSettings.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Scenes.Main_Menu.UI
{
    public class SensitivitySlider : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private TMP_Text value;

        [SerializeField]
        private Slider slider;
        
        [SerializeField]
        private GameEvent saveSettingsEvent;

        [Header("settings")]
        [SerializeField]
        private SensType type;

        private GameSettingsIO _gameSettingsIO;

        private float _prev;

        private void Awake()
        {
            _gameSettingsIO = GetComponent<GameSettingsIO>();
        }

        private void Start()
        {
            slider.value = type switch
            {
                SensType.Horizontal => _gameSettingsIO.PlayerSensitivitySettings.HorizontalMouseSensitivity,
                SensType.Vertical => _gameSettingsIO.PlayerSensitivitySettings.VerticalMouseSensitivity,
                _ => slider.value
            };
            _prev = slider.value;
        }

        private void Update()
        {
            value.text = $"{slider.value.ToString("0.00")}";
            switch (type)
            {
                case SensType.Horizontal:
                    _gameSettingsIO.PlayerSensitivitySettings.HorizontalMouseSensitivity = slider.value;
                    break;
                case SensType.Vertical:
                    _gameSettingsIO.PlayerSensitivitySettings.VerticalMouseSensitivity = slider.value;
                    break;
                default:
                    SwitchUtil.Unreachable();
                    break;
            }


            if (Math.Abs(slider.value - _prev) > 0.01f)
            {
                saveSettingsEvent.Raise(GameEvent.NoData());
            }

            _prev = slider.value;
        }
    }

    public enum SensType
    {
        Horizontal,
        Vertical
    }
}