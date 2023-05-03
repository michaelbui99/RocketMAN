using System;
using UnityEngine;

namespace Modules.Characters.AmmoBot.Scripts
{
    public class AmmoBotAnimator : MonoBehaviour
    {
        private CharacterController _characterController;
        private Animator _animator;
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            _animator.SetFloat(Speed, _characterController.velocity.magnitude);
        }
    }
}