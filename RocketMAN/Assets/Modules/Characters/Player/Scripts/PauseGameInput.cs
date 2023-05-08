using Modules.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Characters.Player.Scripts
{
    public class PauseGameInput : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameEvent pauseGameEvent;
        
        public void PauseGame(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                pauseGameEvent.Raise(GameEvent.NoData());
            }
        }
    }
}
