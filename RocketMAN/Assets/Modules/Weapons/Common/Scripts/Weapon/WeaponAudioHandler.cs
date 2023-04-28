using UnityEngine;

namespace Modules.Weapons.Common.Scripts.Weapon
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
            _audioSource.PlayOneShot(fireWeaponAudio, 0.7f);
        }
        
        public void PlayReload()
        {
            _audioSource.PlayOneShot(reloadWeaponAudio, 0.7f);
        }
        
        public void PlayAlternateFire()
        {
            _audioSource.PlayOneShot(alternateFireAudio, 0.7f);
        }

        public void StopPlaying()
        {
            _audioSource.Stop();
        }
    }
}
