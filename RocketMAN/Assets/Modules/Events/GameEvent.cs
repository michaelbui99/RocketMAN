using System.Collections.Generic;
using UnityEngine;

namespace Modules.Events
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "Game Event")]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventObserver> _observers = new();

        public void Raise()
        {
            _observers.ForEach(o => o.OnEventRaised(null));
        }

        public void Raise(object data)
        {
            _observers.ForEach(o => o.OnEventRaised(data));
        }

        public void RegisterObserver(GameEventObserver observer)
        {
            _observers.Add(observer);
        }

        public void UnregisterObserver(GameEventObserver observer)
        {
            _observers.Remove(observer);
        }

        public static object NoData()
        {
            return null;
        }
    }
}