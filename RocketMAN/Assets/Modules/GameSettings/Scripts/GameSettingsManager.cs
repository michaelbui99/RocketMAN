using System;
using Modules.Events;
using UnityEngine;
using UnityEngine.Audio;

namespace Modules.GameSettings.Scripts
{
    public class GameSettingsManager : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField]
        public AudioMixerGroup GameAudioMixer { get; set; }

        [field: SerializeField]
        public AudioMixerGroup MusicMixer { get; set; }

        private GameSettingsIO _gameSettingsIO;
        private GameEventObserver _saveSettingsObserver;

        private const float MinVolumeValue = -80;

        private void Awake()
        {
            _gameSettingsIO = GetComponent<GameSettingsIO>();
            _saveSettingsObserver = GetComponent<GameEventObserver>();

            var alreadyInstantiated = GameObject.FindGameObjectsWithTag("Settings").Length > 1;

            if (alreadyInstantiated)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }


        private void Update()
        {
        }

        public void SaveSettings()
        {
            _gameSettingsIO.SaveSettings();
        }

        private void HandleAudio()
        {
            GameAudioMixer.audioMixer.SetFloat("GameVolume", _gameSettingsIO.AudioSettings.GameVolume);
            MusicMixer.audioMixer.SetFloat("MusicVolume", _gameSettingsIO.AudioSettings.MusicVolume);

            if (_gameSettingsIO.AudioSettings.DisableMusic)
            {
                MusicMixer.audioMixer.SetFloat("MusicVolume", MinVolumeValue);
            }

            if (_gameSettingsIO.AudioSettings.DisableGameSound)
            {
                GameAudioMixer.audioMixer.SetFloat("GameVolume", MinVolumeValue);
            }
        }
    }
}