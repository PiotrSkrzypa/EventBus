using System;

namespace PSkrzypa.EventBus.EventSubscriber
{
    public class WeakEventSubscriberFactory : IEventSubscriberFactory
    {
        public IEventSubscriber CreateSubscriber<T>(Type payloadType, Action<T> callback, Predicate<T> predicate = null) 
            where T : IEventPayload
        {
            return new WeakEventSubscriber<T>(payloadType, callback, predicate);
        }
    }
}