using System;
using System.Runtime.ExceptionServices;
using Debug = UnityEngine.Debug;

namespace PSkrzypa.EventBus.EventSubscriber
{
    public class EventSubscriber<T> : IEventSubscriber where T : IEventPayload
    {
        public int Id { get; }
        public Type PayloadType { get; }

        private Action<T> _callback;

        private Predicate<T> _predicate;



        public bool IsAlive
        {
            get
            {
                if (_callback == null)
                {
                    return false;
                }
                return true;
            }
        }

        public EventSubscriber(Type payloadType, Action<T> callback, Predicate<T> predicate = null)
        {
            // validate params
            if (payloadType == null)
            {
                throw new ArgumentNullException(nameof(payloadType));
            }
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            // assign values to vars
            PayloadType = payloadType;
            Id = callback.GetHashCode();
            _callback = callback;
            _predicate = predicate;
           

        }
        public void Invoke(T payload)
        {
            // validate callback method
            if (_callback == null)
            {
                Debug.LogError($"{nameof(_callback)} is null.");
                return;
            }
           

            if (_predicate != null)
            {
                // check if predicate returned 'true'
                try
                {
                    var isAccepted = _predicate.Invoke(payload);
                    if (!isAccepted)
                    {
                        Debug.LogWarning($"{nameof(_predicate)} prevented calling {nameof(_callback)}");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    // Preserve the original exception's stack trace when re-throwing
                    Exception actualException = ex.InnerException ?? ex;
                    ExceptionDispatchInfo.Capture(actualException).Throw();
                }
            }

            // invoke callback method
            try
            {
                _callback(payload);
            }
            catch (Exception ex)
            {
                // Preserve the original exception's stack trace when re-throwing
                Exception actualException = ex.InnerException ?? ex;
                ExceptionDispatchInfo.Capture(actualException).Throw();
            }
        }

        public void Dispose()
        {
            _callback = null;

            _predicate = null;
           
        }

        public void Invoke(IEventPayload payload)
        {
            Invoke((T)payload);
        }
    }
}