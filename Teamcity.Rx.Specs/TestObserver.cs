using System;
using System.Collections.Generic;

namespace Teamcity.Rx.Specs
{
    internal class TestObserver<T> : IObserver<T>
    {
        private readonly List<T> _observedEvents;
        private readonly List<Exception> _errors;

        public TestObserver()
        {
            _observedEvents = new List<T>();
            _errors = new List<Exception>();
        }

        public List<T> ObservedEvents
        {
            get { return _observedEvents; }
        }

        public List<Exception> Errors
        {
            get { return _errors; }
        }

        public bool Completed { get; private set; }

        public void OnNext(T value)
        {
            ObservedEvents.Add(value);
        }

        public void OnError(Exception error)
        {
            Errors.Add(error);
        }

        public void OnCompleted()
        {
            Completed = true;
        }
    }
}