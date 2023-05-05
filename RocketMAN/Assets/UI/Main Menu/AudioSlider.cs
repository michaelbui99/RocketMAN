using System;
using Modules.Events;
using Modules.GameSettings.Scripts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.Main_Menu
{
    public class AudioSlider : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private TMP_Text value;

        [SerializeField]
        private Slider slider;

        [SerializeField]
        private GameEvent saveSettingsEvent;

        [Header("Settings")]
        [SerializeField]
        private AudioSliderType type;

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
                AudioSliderType.Game => Mathf.Pow(10, _gameSettingsIO.AudioSettings.GameVolume / 20f),
                AudioSliderType.Music => Mathf.Pow(10, _gameSettingsIO.AudioSettings.MusicVolume / 20f),
                _ => SwitchUtil.Unreachable<float>()
            };
            _prev = slider.value;
        }

        void Update()
        {
            value.text = $"{slider.value.ToString("0.00")}";
            // NOTE: (mibui 2023-05-05) Snippet from here https://johnleonardfrench.com/the-right-way-to-make-a-volume-slider-in-unity-using-logarithmic-conversion/

            switch (type)
            {
                case AudioSliderType.Game:
                    _gameSettingsIO.AudioSettings.GameVolume = Mathf.Log10(slider.value) * 20;
                    break;
                case AudioSliderType.Music:
                    _gameSettingsIO.AudioSettings.MusicVolume = Mathf.Log10(slider.value) * 20;
                    break;
                default:
                    SwitchUtil.Unreachable();
                    break;
            }


            if (Math.Abs(slider.value - _prev) > 0.01f)
            {
                saveSettingsEvent.Raise(null);
            }

            _prev = slider.value;
        }
    }

    public enum AudioSliderType
    {
        Game,
        Music
    }
}