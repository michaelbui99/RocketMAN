using UnityEngine;

namespace Utility.Event
{
    public class EventData<TEventData>
    {
        public Component Emitter{ get; set; }
        public TEventData Data{ get; set; }
    }
}