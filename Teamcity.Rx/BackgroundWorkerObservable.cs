using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Teamcity.Rx
{
    internal abstract class BackgroundWorkerObservable<T> : IObservable<T>
    {
        private readonly BackgroundWorker _worker;
        private readonly List<IObserver<T>> _observers = new List<IObserver<T>>();
        private readonly object _observersLock = new object();
        private bool _doWork;

        protected BackgroundWorkerObservable()
        {
            _worker = new BackgroundWorker();
            _worker.DoWork += (s, e) => Work();
        }

        protected abstract T GetObservableEvents();

        protected void Start()
        {
            _doWork = true;
            _worker.RunWorkerAsync();
        }

        protected void Stop()
        {
            _worker.CancelAsync();
            _doWork = false;
        }

        private void Work()
        {
            while (_doWork)
            {
                var events = GetObservableEvents();

                PushToObservers(o => o.OnNext(events));
            }
        }

        protected void OnError(Exception ex)
        {
            PushToObservers(o => o.OnError(ex));
        }

        private void PushToObservers(Action<IObserver<T>> action)
        {
            lock (_observersLock)
            {
                foreach (var observer in _observers)
                {
                    action(observer);
                }
            }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            lock(_observersLock)
            {
                _observers.Add(observer);
            }

            return new ActionDisposable(() => Remove(observer));
        }

        private void Remove(IObserver<T> observer)
        {
            lock (_observersLock)
            {
                if (_observers.Contains(observer))
                {
                    _observers.Remove(observer);
                }
            }
        }
    }
}