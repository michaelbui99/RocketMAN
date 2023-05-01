using System.Collections;
using UnityEngine;

namespace Modules.Shared.Cooldown
{
    public class CooldownHandler : MonoBehaviour
    {
        private bool _cooldownActive = false;

        public delegate void CooldownEventTrigger();

        public event CooldownEventTrigger CooldownFinishedEvent;
        public event CooldownEventTrigger CooldownStartedEvent;

        public bool CooldownActive() => _cooldownActive;

        private Coroutine _cooldownRoutine;

        public void StartCooldown(float cooldownInSeconds)
        {
            _cooldownRoutine = StartCoroutine(Cooldown(cooldownInSeconds));
        }

        public void ForceResetCooldown()
        {
            _cooldownActive = false;
            if (_cooldownRoutine != null)
            {
                StopCoroutine(_cooldownRoutine);
            }
            CooldownFinishedEvent?.Invoke();
        }

        private IEnumerator Cooldown(float seconds)
        {
            CooldownStartedEvent?.Invoke();
            _cooldownActive = true;
            yield return new WaitForSeconds(seconds);
            _cooldownActive = false;
            CooldownFinishedEvent?.Invoke();
            _cooldownRoutine = null;
        }
    }
}