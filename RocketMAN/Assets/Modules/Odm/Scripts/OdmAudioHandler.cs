using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.Odm.Scripts
{
    public class OdmAudioHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private AudioClip gasAudio;

        [SerializeField]
        private AudioClip fireHookAudio;

        private IGasController _gasController;

        private readonly Dictionary<AudioClip, AudioSource> _audioSources = new();

        private void OnEnable()
        {
            _audioSources.Keys.ToList()
                .ForEach(k => { _audioSources[k].enabled = true; });
        }

        private void Awake()
        {
            _gasController = GetComponent<IGasController>();
            _gasController.OnGasActivated += PlayGasSound;
            _gasController.OnGasReleased += StopGasSound;
        }

        private void OnDisable()
        {
            _audioSources.Keys.ToList()
                .ForEach(k => _audioSources[k].enabled = false);
        }

        private void OnDestroy()
        {
            _gasController.OnGasActivated -= PlayGasSound;
            _gasController.OnGasReleased -= StopGasSound;
        }

        private void PlayGasSound()
        {
            if (!_audioSources.TryGetValue(gasAudio, out var gasAudioSource))
            {
                gasAudioSource = gameObject.AddComponent<AudioSource>();
                _audioSources.Add(gasAudio, gasAudioSource);
            }

            gasAudioSource.clip = gasAudio;
            gasAudioSource.enabled = true;
            gasAudioSource.loop = true;
            gasAudioSource.time = 0.8f;
            gasAudioSource.Play();
        }

        private void StopGasSound()
        {
            if (!_audioSources.TryGetValue(gasAudio, out var gasAudioSource))
            {
                return;
            }

            gasAudioSource.enabled = false;
            gasAudioSource.loop = false;
        }
    }
}