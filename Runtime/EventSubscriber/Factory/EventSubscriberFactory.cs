using System;

namespace PSkrzypa.EventBus.EventSubscriber
{
    public class EventSubscriberFactory : IEventSubscriberFactory
    {
        public IEventSubscriber CreateSubscriber<T>(Type payloadType, Action<T> callback, Predicate<T> predicate = null) 
            where T : IEventPayload
        {
            return new EventSubscriber<T>(payloadType, callback, predicate);
        }
    }
}