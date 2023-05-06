using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Modules.Characters.AmmoBot.Scripts.States
{
    public class ResupplyPlayerState : IAmmoBotState
    {
        public IEnumerator Act(AmmoBotAI bot)
        {
            if (bot.SpawnedSupplyCrates.Count >= bot.ClearAllCratesThreshold)
            {
                bot.ClearAllSupplyCrates();
            }

            if (Vector3.Distance(bot.transform.position, bot.Player.transform.position) > bot.MinDistanceToPlayer
                || bot.SpawnedSupplyCrates.Count >= bot.CratesToDropOnResupply || !bot.PlayerNeedsResupply())
            {
                yield return new WaitForSeconds(5);
                bot.SwitchTo(new IdleState());
                yield return null;
            }

            bot.SpawnSupplyCrate(bot.transform.position);
            yield return new WaitForSeconds(1);
            bot.SwitchTo(new IdleState());
            yield return null;
        }
    }
