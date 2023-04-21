namespace Utility.Event
{
    public interface IEventEmitter<TEventData>
    {
        public void Subscribe(IEventObserver<TEventData> observer);
        public void Unsubscribe(IEventObserver<TEventData> observer);
        public void EmitEvent(EventData<TEventData> eventData);
    }
}