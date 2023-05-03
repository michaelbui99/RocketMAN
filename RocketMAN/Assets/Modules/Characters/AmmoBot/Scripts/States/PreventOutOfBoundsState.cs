using System.Collections;
using UnityEngine;

namespace Modules.Characters.AmmoBot.Scripts.States
{
    public class PreventOutOfBoundsState : IAmmoBotState
    {
        public IEnumerator Act(AmmoBotAI bot)
        {
            while (Vector3.Distance(bot.transform.position, bot.ReturnToBoundsPoint.position) > 1f)
            {
                var botTransform = bot.transform;
                var target = bot.ReturnToBoundsPoint.position;
                botTransform.LookAt(target);
                var direction = (target - botTransform.position).normalized;
                bot.CharacterController.SimpleMove(direction * bot.MovementSpeed);
                yield return new WaitForSeconds(0.1f);
            }

            bot.SwitchTo(new IdleState(5));
            yield return null;
        }
    }
}