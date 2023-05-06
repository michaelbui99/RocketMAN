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

        private void Start()
        {
            var loadedSens = TryRetrieveSettings(SensitivitySettingsKey);
            var loadedAudio = TryRetrieveSettings(AudioSettingsKey);

            Debug.Log($"LOADED SENS: {loadedSens}, LOADED AUDIO: {loadedAudio}");
        }

        public void SaveSettings()
        {
            Debug.Log($"SENS SETTINGS: {JsonUtility.ToJson(PlayerSensitivitySettings.ToSerializable())}");
            Debug.Log($"AUDIO SETTINGS: {JsonUtility.ToJson(AudioSettings.ToSerializable())}");
            PlayerPrefs.SetString(SensitivitySettingsKey,
                JsonUtility.ToJson(PlayerSensitivitySettings.ToSerializable()));
            PlayerPrefs.SetString(AudioSettingsKey, JsonUtility.ToJson(AudioSettings.ToSerializable()));
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
                return false;
            }

            switch (key)
            {
                case SensitivitySettingsKey:
                    var savedSensSettings =
                        JsonUtility.FromJson<SerializableSensitivitySettings>(
                            PlayerPrefs.GetString(SensitivitySettingsKey));
                    ApplySensitivitySettings(savedSensSettings);
                    break;
                case AudioSettingsKey:
                    var savedAudioSettings =
                        JsonUtility.FromJson<SerializableAudioSettings>(PlayerPrefs.GetString(AudioSettingsKey));
                    ApplyAudioSettings(savedAudioSettings);
                    break;
                default:
                    SwitchUtil.Unreachable();
                    break;
            }

            return true;
        }

        private void ApplySensitivitySettings(SerializableSensitivitySettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentException("Settings cannot be null");
            }

            PlayerSensitivitySettings.HorizontalMouseSensitivity = settings.HorizontalMouseSensitivity;
            PlayerSensitivitySettings.VerticalMouseSensitivity = settings.VerticalMouseSensitivity;
        }

        private void ApplyAudioSettings(SerializableAudioSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentException("Settings cannot be null");
            }

            AudioSettings.MusicVolume = settings.MusicVolume;
            AudioSettings.DisableMusic = settings.DisableMusic;

            AudioSettings.GameVolume = settings.GameVolume;
            AudioSettings.DisableGameSound = settings.DisableGameSound;
        }
    }

    public class SerializableAudioSettings
    {
        public float MusicVolume;
        public float GameVolume;
        public bool DisableMusic;
        public bool DisableGameSound;
    }

    public class SerializableSensitivitySettings
    {
        public float HorizontalMouseSensitivity;
        public float VerticalMouseSensitivity;
    }

    public static class SettingsExtensions
    {
        public static SerializableAudioSettings ToSerializable(this AudioSettings settings)
        {
            return new SerializableAudioSettings
            {
                GameVolume = settings.GameVolume,
                DisableMusic = settings.DisableMusic,
                MusicVolume = settings.MusicVolume,
                DisableGameSound = settings.DisableGameSound
            };
        }

        public static SerializableSensitivitySettings ToSerializable(this SensitivitySettings settings)
        {
            return new SerializableSensitivitySettings
            {
                HorizontalMouseSensitivity = settings.HorizontalMouseSensitivity,
                VerticalMouseSensitivity = settings.VerticalMouseSensitivity
            };
        }
    }
}