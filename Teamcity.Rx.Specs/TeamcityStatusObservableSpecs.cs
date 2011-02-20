using System;
using System.Configuration;
using System.Threading;
using Machine.Specifications;

namespace Teamcity.Rx.Specs
{
    public class When_I_get_the_teamcity_status
    {
        static TeamcityStatusObservable StatusObservable;
        static TestObserver<string> StatusObserver;

        Establish context = () => 
        {
            StatusObservable = new TeamcityStatusObservable(ConfigurationManager.AppSettings["hostname"], ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]);
            StatusObserver = new TestObserver<string>();
        };

        Because we_observed_the_teamcity_status = () =>
        {
            StatusObservable.Subscribe(StatusObserver);
            Thread.Sleep(new TimeSpan(0, 0, 15));
        };

        It should_return_some_statuses = () => StatusObserver.ObservedEvents.Count.ShouldBeGreaterThan(0);
    }
}