using System;
using UnityEngine;

namespace Modules.Weapons.Common.Scripts
{
    public class WeaponAudioHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private AudioClip fireWeaponAudio;
        [SerializeField]
        private AudioClip alternateFireAudio;
        [SerializeField]
        private AudioClip reloadWeaponAudio;
    
        private AudioSource _audioSource;

        private void OnEnable()
        {
            _audioSource.enabled = true;
            _audioSource.loop = false;
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnDisable()
        {
            _audioSource.enabled = false;
        }

        public void PlayFireWeapon()
        {
            _audioSource.clip = fireWeaponAudio;
            _audioSource.Play();
        }
        
        public void PlayReload()
        {
            _audioSource.clip = reloadWeaponAudio;
            _audioSource.Play();
        }
        
        public void PlayAlternateFire()
        {
            _audioSource.clip = alternateFireAudio;
            _audioSource.Play();
        }

        public void StopPlaying()
        {
            _audioSource.Stop();
        }
    }
}
