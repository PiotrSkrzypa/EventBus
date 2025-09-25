using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using Zenject;
using Debug = UnityEngine.Debug;

namespace PSkrzypa.EventBus
{
    public class MainThreadDispatcher : IThreadDispatcher, ITickable
    {
        private readonly ConcurrentQueue<DispatcherTask> _tasks = new ConcurrentQueue<DispatcherTask>();

        public int ThreadId
        {
            get;
            private set;
        }

        public int TasksCount => _tasks.Count;


        public MainThreadDispatcher()
        {
            ThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public void Dispatch(Delegate action, object[] payload)
        {
            _tasks.Enqueue(new DispatcherTask(action, payload));
        }

        public void Tick()
        {
            while (_tasks.Count > 0)
            {
                if (!_tasks.TryDequeue(out var task))
                {
                    continue;
                }
                Debug.Log($"(Queue.Count: {_tasks.Count}) Dispatching task {task.Action}");

                task.Invoke();
                task.Dispose();
            }
        }
    }
}