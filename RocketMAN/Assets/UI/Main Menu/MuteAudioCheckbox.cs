using System;
using Modules.Events;
using Modules.GameSettings.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.Main_Menu
{
    public class MuteAudioCheckbox : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameEvent saveSettingsEvent;

        [Header("Settings")]
        [SerializeField]
        private MuteTarget muteTarget;

        private GameSettingsIO _gameSettingsIO;
        private Toggle _toggle;

        private void Awake()
        {
            _gameSettingsIO = GetComponent<GameSettingsIO>();
            _toggle = GetComponent<Toggle>();
        }

        private void Start()
        {
            _toggle.isOn = muteTarget switch
            {
                MuteTarget.Game => _gameSettingsIO.AudioSettings.DisableGameSound,
                MuteTarget.Music => _gameSettingsIO.AudioSettings.DisableMusic,
                _ => SwitchUtil.Unreachable<bool>()
            };
        }

        public void Toggle()
        {
            switch (muteTarget)
            {
                case MuteTarget.Game:
                    _gameSettingsIO.AudioSettings.DisableGameSound = _toggle.isOn;
                    break;
                case MuteTarget.Music:
                    _gameSettingsIO.AudioSettings.DisableMusic = _toggle.isOn;
                    break;
                default:
                    SwitchUtil.Unreachable();
                    break;
            }

            saveSettingsEvent.Raise(GameEvent.NoData());
        }
    }

    public enum MuteTarget
    {
        Game,
        Music
    }
}