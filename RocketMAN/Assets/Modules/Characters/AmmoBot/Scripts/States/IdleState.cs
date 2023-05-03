using System.Collections;
using UnityEngine;

namespace Modules.Characters.AmmoBot.Scripts.States
{
    public class IdleState : IAmmoBotState
    {
        public IEnumerator Act(AmmoBotAI bot)
        {
            while (Vector3.Distance(bot.transform.position, bot.Player.transform.position) < bot.MinDistanceToPlayer)
            {
                Transform botTransform = bot.transform;
                botTransform.LookAt(bot.Player.transform.position);
                botTransform.rotation = Quaternion.Euler(0f, botTransform.rotation.eulerAngles.y, 0);
                yield return new WaitForSeconds(0.1f);
            }

            bot.SwitchTo(new SeekPlayerState());
            yield return null;
        }
    }
}