using System;
using System.Collections.Generic;
using System.Linq;
using Teamcity.Rx.Events;

namespace Teamcity.Rx
{
    internal class TeamcityEventObservable : IObservable<ITeamcityEvent>
    {
        private readonly IEnumerable<ITeamcityEvent> _events;

        public TeamcityEventObservable(IEnumerable<Project> teamcity, IEnumerable<Project> projects)
        {
            _events = DiffAllProjects(teamcity.ToList(), projects.ToList());
        }

        public IEnumerable<ITeamcityEvent> DiffAllProjects(List<Project> oldProjects, List<Project> newProjects)
        {
            //Projects added = all projects that are in the new projects but not in the old projects
            foreach (var project in newProjects.Where(project => false == oldProjects.Exists(o => o.Name == project.Name)))
            {
                yield return new ProjectAdded(project);
            }

            //Projects removed = all projects that were in the old projects but not in the new projects
            foreach (var project in oldProjects.Where(project => false == newProjects.Exists(o => o.Name == project.Name)))
            {
                yield return new ProjectRemoved(project);
            }

            foreach (var difference in from project in oldProjects.Intersect(newProjects)
                                     let oldProject = oldProjects.First(s => s.Id.Equals(project.Id))
                                     let newProject = newProjects.First(s => s.Id.Equals(project.Id))
                                     from difference in DiffProject(oldProject, newProject)
                                     select difference)
            {
                yield return difference;
            }
        }

        private IEnumerable<ITeamcityEvent> DiffProject(Project oldProject, Project newProject)
        {
            yield break;
        }


        public IDisposable Subscribe(IObserver<ITeamcityEvent> observer)
        {
            foreach (var @event in _events)
            {
                observer.OnNext(@event);
            }

            observer.OnCompleted();

            return new EmptyDisposable();
        }
    }
}