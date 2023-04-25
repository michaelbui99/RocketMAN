using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Modules.MusicManager.Scripts
{
    public class MusicManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private MusicSettings settings;

        private List<AudioClip> _allSongs;
        private Queue<AudioClip> _queue = new();
        private AudioSource _audio;
        private AudioClip _currentSong;

        private const float MinAudioLevel = 0f;
        private const float MaxAudioLevel = 100f;

        private bool _active = false;

        private void OnEnable()
        {
            _allSongs = Resources.LoadAll("Songs", typeof(AudioClip))
                .Cast<AudioClip>()
                .ToList();
            _audio = GetComponent<AudioSource>();
            _audio.playOnAwake = false;
            _audio.loop = false;
               
            StartPlayer();
        }

        private void Awake()
        {
            var alreadyInstantiated = GameObject.FindGameObjectsWithTag("Music").Length > 1;

            if (alreadyInstantiated)
            {
                Destroy(gameObject);
            }
            
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (!_active)
            {
                return;
            }

            if (_audio.time >= _currentSong.length)
            {
                _currentSong = GetNextSong();
            }
        }

        private void StartPlayer()
        {
            _audio.Stop();
            
            QueueSongs();

            if (_queue.Count == 0)
            {
                return;
            }

            _currentSong = GetNextSong();
            _active = true;
            _audio.PlayOneShot(_currentSong, TranslateAudioLevel(settings.AudioLevel));
        }

        private void QueueSongs()
        {
            _allSongs.ForEach(s => _queue.Enqueue(s));
        }

        private void StopPlayer()
        {
            _audio.Stop();
            _active = false;
        }

        private AudioClip GetNextSong()
        {
            if (_queue.Count == 0)
            {
                QueueSongs();
            }

            return _queue.Dequeue();
        }
        
        private float TranslateAudioLevel(float audioLevel)
        {
            return audioLevel switch
            {
                < MinAudioLevel => MinAudioLevel,
                > MaxAudioLevel => MaxAudioLevel,
                _ => audioLevel / 100
            };
        }
    }
}