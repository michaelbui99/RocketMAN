using System;
using System.Threading;
using Modules.Characters.Player.Scripts;
using Modules.MusicManager.Scripts;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using Utility;
using AudioSettings = Modules.MusicManager.Scripts.AudioSettings;

namespace Modules.GameSettings.Scripts
{
    public class GameSettingsIO : MonoBehaviour
    {
        [field: Header("Player Settings Refs")]
        [field: SerializeField]
        public SensitivitySettings PlayerSensitivitySettings { get; set; }

        [field: SerializeField]
        public AudioSettings AudioSettings { get; set; }

        public const string SensitivitySettingsKey = "SENS_SETTINGS";
        public const string AudioSettingsKey = "AUDIO_SETTINGS";

        private void Awake()
        {
            try
            {
                TryRetrieveSettings(SensitivitySettingsKey);
                TryRetrieveSettings(AudioSettingsKey);
            }
            catch (Exception e)
            {
                SaveSettings();
            }
        }

        public void SaveSettings()
        {
            PlayerPrefs.SetString(SensitivitySettingsKey, JsonUtility.ToJson(PlayerSensitivitySettings));
            PlayerPrefs.SetString(AudioSettingsKey, JsonUtility.ToJson(AudioSettings));
        }

        public bool TryRetrieveSettings(string key)
        {
            var exists = key switch
            {
                SensitivitySettingsKey => true,
                AudioSettingsKey => true,
                _ => false
            };

            if (!exists)
            {
                Debug.Log("HERE");
                return false;
            }

            switch (key)
            {
                case SensitivitySettingsKey:
                    var savedSensSettings =
                        JsonUtility.FromJson<SensitivitySettings>(PlayerPrefs.GetString(SensitivitySettingsKey));
                    ApplySensitivitySettings(savedSensSettings);
                    break;
                case AudioSettingsKey:
                    var savedAudioSettings =
                        JsonUtility.FromJson<AudioSettings>(PlayerPrefs.GetString(AudioSettingsKey));
                    ApplyAudioSettings(savedAudioSettings);
                    break;
                default:
                    SwitchUtil.Unreachable();
                    break;
            }

            return true;
        }

        private void ApplySensitivitySettings(SensitivitySettings settings)
        {
            PlayerSensitivitySettings.HorizontalMouseSensitivity = settings.HorizontalMouseSensitivity;
            PlayerSensitivitySettings.VerticalMouseSensitivity = settings.VerticalMouseSensitivity;
        }

        private void ApplyAudioSettings(AudioSettings settings)
        {
            AudioSettings.MusicVolume = settings.MusicVolume;
            AudioSettings.DisableMusic = settings.DisableMusic;
        }
    }
}