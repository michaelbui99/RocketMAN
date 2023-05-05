using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Modules.AudioMixerProvider.Scripts
{
    public class AudioMixerProvider : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private AudioMixerGroup gameAudioMixer;

        [SerializeField]
        private AudioMixerGroup musicAudioMixer;

        [Header("Settings")]
        [SerializeField]
        private float pollIntervalTimeInSeconds;

        private void Start()
        {
            StartCoroutine(StartProvider());
        }

        private void Update()
        {
        }

        private IEnumerator StartProvider()
        {
            while (true)
            {
                ProvideMixers();
                yield return new WaitForSeconds(pollIntervalTimeInSeconds);
            }
        }

        private void ProvideMixers()
        {
            var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            rootObjects
                .Where(o => o.GetComponentInChildren<AudioSource>() != null)
                .ToList()
                .ForEach(o =>
                {
                    var audioSource = o.GetComponentInChildren<AudioSource>();
                    audioSource.outputAudioMixerGroup = o.name switch
                    {
                        "MusicManager" => musicAudioMixer,
                        _ => gameAudioMixer
                    };
                });

            // NOTE: (mibui 2023-05-05) This is required since the MusicManager is marked as "DontDestroyOnLoad" 
            //                          and is not found with GetRootGameObjects
            var musicManagerSource = GameObject.Find("MusicManager").GetComponentInChildren<AudioSource>();
            if (musicManagerSource != null)
            {
                musicManagerSource.outputAudioMixerGroup = musicAudioMixer;
            }
        }
    }
}