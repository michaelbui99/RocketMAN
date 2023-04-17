using System;
using UnityEngine;
using Weapons.Common;
using Weapons.Common.Scripts;

namespace Weapons.RocketLauncher.Scripts
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