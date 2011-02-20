using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Teamcity.Rx.Events;
using Teamcity.Rx.Parser;

namespace Teamcity.Rx
{
    public class TeamcityObservable : IObservable<ITeamcityEvent>
    {
        private readonly Teamcity _teamcity;
        private readonly IObservable<ITeamcityEvent> _events;

        public Teamcity Teamcity
        {
            get { return _teamcity; }
        }

        public TeamcityObservable(string teamcityUrl, string username, string password)
        {
            _teamcity = new Teamcity();
            _events = from status in new TeamcityStatusObservable(teamcityUrl, username, password)
                      from projects in new TeamcityStatusParser(status)
                      from teamcityEvent in new TeamcityEventObservable(_teamcity, projects)
                      select teamcityEvent;
        }

        public IDisposable Subscribe(IObserver<ITeamcityEvent> observer)
        {
            return _events.Subscribe(observer);
        }
    }
}