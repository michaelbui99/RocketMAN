using System.Collections;
using UnityEngine;

namespace Modules.Characters.AmmoBot.Scripts.States
{
    public class IdleState : IAmmoBotState
    {
        private readonly float _idleTime;

        public IdleState()
        {
            _idleTime = 0;
        }

        public IdleState(float idleTime)
        {
            _idleTime = idleTime;
        }

        public IEnumerator Act(AmmoBotAI bot)
        {
            bot.CharacterController.SimpleMove(Vector3.zero);
            yield return new WaitForSeconds(_idleTime);
            while (Vector3.Distance(bot.transform.position, bot.Player.transform.position) < bot.MinDistanceToPlayer)
            {
                bot.CharacterController.SimpleMove(Vector3.zero);
                yield return new WaitForSeconds(_idleTime);
            }

            bot.SwitchTo(new SeekPlayerState());
            yield return null;
        }
    }
}