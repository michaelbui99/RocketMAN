namespace Utility.Event
{
    public interface IEventObserver<TEventData>
    {
        public void OnEventEmitted(EventData<TEventData> eventData);
    }
}