using System.Collections;

namespace Modules.Characters.AmmoBot.Scripts.States
{
    public interface IAmmoBotState
    {
        public IEnumerator Act(AmmoBotAI bot);
    }
}