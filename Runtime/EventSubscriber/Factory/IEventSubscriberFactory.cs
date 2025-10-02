using System;

namespace PSkrzypa.EventBus.EventSubscriber
{
    public interface IEventSubscriberFactory
    {
        IEventSubscriber CreateSubscriber<T>(Type payloadType, Action<T> callback, Predicate<T> predicate = null) where T : IEventPayload;
    }
}