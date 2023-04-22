using System;
using UnityEngine;
using UnityEngine.Events;

namespace Modules.Events
{
    public class GameEventObserver: MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameEvent gameEvent;
        [SerializeField]
        private OnEvent onEvent;
        
        private void OnEnable()
        {
            gameEvent.RegisterObserver(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterObserver(this);
        }

        public void OnEventRaised(object data)
        {
            onEvent?.Invoke(data);
        }

        public void RegisterCallback(UnityAction<object> cb)
        {
            onEvent.AddListener(cb); 
        }
    }
}