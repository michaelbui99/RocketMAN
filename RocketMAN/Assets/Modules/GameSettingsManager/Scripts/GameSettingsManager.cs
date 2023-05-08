using System;
using System.Collections;
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

        [Header("Settings")]
        [SerializeField]
        private float saveInterval = 5f;

        private GameSettingsIO _gameSettingsIO;
        private GameEventObserver _saveSettingsObserver;

        private const float MinVolumeValue = -80;
        private bool _shouldSave = true;

        private void Awake()
        {
            _gameSettingsIO = GetComponent<GameSettingsIO>();
            _saveSettingsObserver = GetComponent<GameEventObserver>();

            var alreadyInstantiated = GameObject.FindGameObjectsWithTag("Settings").Length > 1;

            if (alreadyInstantiated)
            {
                Destroy(gameObject);
            }

            _saveSettingsObserver.RegisterCallback(SaveSettingsCb);
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            _saveSettingsObserver.UnregisterCallback(SaveSettingsCb);
        }

        private void Update()
        {
            HandleAudio();

            if (_shouldSave)
            {
                StartCoroutine(DoSave());
            }
        }


        public void SaveSettingsCb(object _)
        {
            SaveSettings();
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

        private IEnumerator DoSave()
        {
            _shouldSave = false;
            SaveSettings();
            yield return new WaitForSeconds(saveInterval);
            _shouldSave = true;
        }
    }
}