using System;
using UnityEngine;
using UnityEngine.AI;

namespace Modules.Characters.AmmoBot.Scripts
{
    public class AmmoBotAnimator : MonoBehaviour
    {
        private Animator _animator;
        private NavMeshAgent _agent;
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            _animator.SetFloat(Speed, _agent.velocity.magnitude);
        }
    }
}