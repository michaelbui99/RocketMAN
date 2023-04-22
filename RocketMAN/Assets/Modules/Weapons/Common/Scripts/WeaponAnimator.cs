using System.Collections;
using UnityEngine;

namespace Modules.Weapons.Common.Scripts
{
    public class WeaponAnimator: MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Shooting = Animator.StringToHash("Shooting");

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
    }
}