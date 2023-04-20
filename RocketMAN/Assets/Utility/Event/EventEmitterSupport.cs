using System.Collections.Generic;

namespace Utility.Event
{
    public class EventEmitterSupport<TEventData>
    {
        private readonly List<IEventObserver<TEventData>> _observers = new();


        public void AddObserver(IEventObserver<TEventData> observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IEventObserver<TEventData> observer)
        {
            _observers.Remove(observer);
        }

        public void EmitEvent(EventData<TEventData> eventData)
        {
            _observers.ForEach(o => o.OnEventEmitted(eventData));
        }
    }
}