using System.Collections;
using UnityEngine;

namespace Modules.Weapons.Common.Scripts.Weapon
{
    public class WeaponAnimator: MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Shooting = Animator.StringToHash("Shooting");
        private static readonly int Reloading = Animator.StringToHash("Reloading");

        private bool reloading = false;
 
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public IEnumerator Fire(float weaponCooldown)
        {
            _animator.SetTrigger(Shooting);
            yield return new WaitForSeconds(weaponCooldown);
            _animator.ResetTrigger(Shooting);
        }

        public void ReloadStarted()
        {
            _animator.SetBool(Reloading, true);
        }

        public void ReloadFinished()
        {
            _animator.SetBool(Reloading, false);
        }
    }
}