using Modules.Characters.Player.Scripts;
using Modules.Events;
using TMPro;
using UnityEngine;
using Utility;

namespace Modules.Characters.Player.UI
{
    public class SpeedUI : MonoBehaviour
    {
        private TMP_Text _speed;
        private GameEventObserver _movementStateObserver;

        private MovementStateEvent _currentKnownMovementState = new()
        {
            Speed = 0
        };

        private void Awake()
        {
            _speed = GetComponent<TMP_Text>();
            _movementStateObserver = GetComponent<GameEventObserver>();

            _movementStateObserver.RegisterCallback(UpdateMovementState);
        }

        private void Update()
        {
            _speed.text = $"{_currentKnownMovementState.Speed.ToString("0.00")} u/s";
        }

        private void OnDestroy()
        {
            _movementStateObserver.UnregisterCallback(UpdateMovementState);
        }

        private void UpdateMovementState(object newState)
        {
            _currentKnownMovementState = newState.Cast<MovementStateEvent>();
        }
    }
}