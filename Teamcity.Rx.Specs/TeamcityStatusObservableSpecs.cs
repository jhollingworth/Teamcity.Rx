using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using Machine.Specifications;

namespace Teamcity.Rx.Specs
{
    internal class TestStatusObserver : IObserver<string>
    {
        public TestStatusObserver()
        {
            Statuses = new List<string>();
            Errors = new List<Exception>();
        }

        public List<string> Statuses { get; set; }
        public List<Exception> Errors { get; set; }

        public void OnNext(string value)
        {
            Statuses.Add(value);    
        }

        public void OnError(Exception error)
        {
            Errors.Add(error);
        }

        public void OnCompleted()
        {
        }
    }

    public class When_I_get_the_teamcity_status
    {
        static TeamcityStatusObservable StatusObservable;
        static TestStatusObserver StatusObserver;

        Establish context = () => 
        {
            StatusObservable = new TeamcityStatusObservable(ConfigurationManager.AppSettings["hostname"], ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]);    
            StatusObserver = new TestStatusObserver();
        };

        Because we_observed_the_teamcity_status = () =>
        {
            StatusObservable.Subscribe(StatusObserver);
            Thread.Sleep(new TimeSpan(0, 0, 15));
        };

        It should_return_some_statuses = () => StatusObserver.Statuses.Count.ShouldBeGreaterThan(0);
    }
}