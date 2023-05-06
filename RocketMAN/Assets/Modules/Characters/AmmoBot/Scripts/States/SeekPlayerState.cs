using System.Collections;
using UnityEngine;

namespace Modules.Characters.AmmoBot.Scripts.States
{
    public class SeekPlayerState : IAmmoBotState
    {
        public IEnumerator Act(AmmoBotAI bot)
        {
            Vector3 botPosition = bot.gameObject.transform.position;
            Vector3 playerPosition = bot.Player.gameObject.transform.position;
            bot.Agent.enabled = true;
            bot.DialogController.Write("Why are you leaving me Rocket Man :(");
            while (Vector3.Distance(botPosition, playerPosition) > bot.MinDistanceToPlayer)
            {
                bot.LookAtPlayer();
                bot.Agent.SetDestination(playerPosition);
                CapMovementSpeed(bot);

                yield return new WaitForSeconds(0.1f);
                botPosition = UpdatePositions(bot, out playerPosition);
            }

            bot.Agent.enabled = false;
            if (bot.PlayerNeedsResupply())
            {
                bot.SwitchTo(new ResupplyPlayerState());
                yield return null;
            }

            bot.SwitchTo(new IdleState());
            yield return null;
        }

        private Vector3 UpdatePositions(AmmoBotAI bot, out Vector3 playerPosition)
        {
            var botPosition = bot.gameObject.transform.position;
            playerPosition = bot.Player.transform.position;
            return botPosition;
        }

        private void CapMovementSpeed(AmmoBotAI bot)
        {
            if (Vector3.SqrMagnitude(bot.Agent.velocity) > Mathf.Pow(bot.MovementSpeed, 2))
            {
                bot.Agent.velocity = bot.Agent.velocity.normalized * bot.MovementSpeed;
            }
        }
    }
}