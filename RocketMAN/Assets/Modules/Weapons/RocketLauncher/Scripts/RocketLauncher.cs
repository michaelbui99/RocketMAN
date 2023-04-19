using Modules.Weapons.Common.Scripts;
using UnityEngine;

namespace Modules.Weapons.RocketLauncher.Scripts
{
    public class RocketLauncher: MonoBehaviour,IWeapon
    {
        private AudioSource _fireRocketAudio;
        private IProjectileLauncher _projectileLauncher;

        private void Awake()
        {
            _fireRocketAudio = GetComponent<AudioSource>();
            _projectileLauncher = GetComponent<IProjectileLauncher>();
        }

        private void Start()
        {
            _fireRocketAudio.enabled = true;
        }

        public void FireWeapon()
        {
            _fireRocketAudio.Play();
            _projectileLauncher.Launch();
        }

        public void ReloadWeapon()
        {
            throw new System.NotImplementedException();
        }
    }
}