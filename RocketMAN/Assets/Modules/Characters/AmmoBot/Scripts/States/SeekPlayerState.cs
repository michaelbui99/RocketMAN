using System.Collections;
using UnityEngine;

namespace Modules.Characters.AmmoBot.Scripts.States
{
    public class SeekPlayerState : IAmmoBotState
    {
        public IEnumerator Act(AmmoBotAI bot)
        {
            Vector3 botPosition = bot.gameObject.transform.position;
            Vector3 playerPosition = bot.Player.transform.position;
            bot.DialogController.Write("Why are you leaving me Rocket Man :(");
            while (Vector3.Distance(botPosition, playerPosition) > bot.MinDistanceToPlayer)
            {
                var botTransform = bot.transform;
                var direction = (playerPosition - botPosition).normalized;
                botTransform.LookAt(playerPosition);
                botTransform.rotation = Quaternion.Euler(0f, botTransform.rotation.eulerAngles.y, 0);
                // NOTE: (mibui 2023-05-03) SimleMove is frame-independent, hence no Time.deltaTime;
                bot.CharacterController.SimpleMove(direction * bot.MovementSpeed);
                yield return new WaitForSeconds(0.2f);
                botPosition = bot.gameObject.transform.position;
                playerPosition = bot.Player.transform.position;
            }

            if (bot.CurrentKnownWeaponState.CurrentAmmo == 0 && bot.CurrentKnownWeaponState.RemainingAmmo == 0)
            {
                bot.SwitchTo(new ProvideAmmoState());
                yield return null;
            }

            bot.SwitchTo(new IdleState());
            yield return null;
        }
    }
}