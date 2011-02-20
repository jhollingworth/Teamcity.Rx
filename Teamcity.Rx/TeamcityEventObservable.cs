using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using log4net;
using Teamcity.Rx.Events;

namespace Teamcity.Rx
{
    internal class TeamcityEventObservable : IObservable<ITeamcityEvent>
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(TeamcityEventObservable));
        private readonly List<ITeamcityEvent> _events;

        public TeamcityEventObservable(Teamcity teamcity, IEnumerable<Project> projects)
        {
            _log.Debug("Finding differences between the old state of teamcity and the new state");

            _events = DiffAllProjects(teamcity.ToList(), projects.ToList()).ToList();
            
            _log.DebugFormat("Found {0} changes", _events.Count);

            teamcity.UpdateProjects(projects);
        }

        private static IEnumerable<ITeamcityEvent> DiffAllProjects(List<Project> oldProjects, List<Project> newProjects)
        {
            //Projects added = all projects that are in the new projects but not in the old projects
            foreach (var project in newProjects.Where(project => false == oldProjects.Exists(o => o.Id.Equals(project.Id))))
            {
                _log.DebugFormat("Project {0} was added", project.Name);

                yield return new ProjectAdded(project);
            }

            //Projects removed = all projects that were in the old projects but not in the new projects
            foreach (var project in oldProjects.Where(project => false == newProjects.Exists(o => o.Id.Equals(project.Id))))
            {
                _log.DebugFormat("Project {0} was removed", project.Name);

                yield return new ProjectRemoved(project);
            }

            var events = from project in oldProjects.Intersect(newProjects)
                         let oldProject = oldProjects.First(s => s.Id.Equals(project.Id))
                         let newProject = newProjects.First(s => s.Id.Equals(project.Id))
                         from @event in DiffProject(oldProject, newProject)
                         select @event;

            foreach (var @event in events)
            {
                yield return @event;
            }
        }

        private static IEnumerable<ITeamcityEvent> DiffProject(Project oldProject, Project newProject)
        {
            var newBuilds = newProject.Builds;
            var oldBuilds = oldProject.Builds;

            //Builds added = all builds that are in the new builds but not in the old builds
            foreach (var build in newBuilds.Where(build => 0 == oldBuilds.Count(b => b.Id.Equals(build.Id))))
            {
                _log.DebugFormat("Build {0} was added", build.Name);

                yield return new BuildAdded(build);
            }

            //Builds removed = all builds that are in the old builds but not in the new builds
            foreach (var build in oldBuilds.Where(build => 0 == newBuilds.Count(b => b.Id.Equals(build.Id))))
            {
                _log.DebugFormat("Build {0} was removed", build.Name);

                yield return new BuildRemoved(build);
            }

            var buildEvents = from unChangedBuild in oldBuilds.Intersect(newBuilds)
                              let oldBuild = oldBuilds.First(b => b.Id.Equals(unChangedBuild.Id))
                              let newBuild = newBuilds.First(b => b.Id.Equals(unChangedBuild.Id))
                              from @event in DiffBuild(oldBuild, newBuild)
                              select @event;

            foreach (var difference in buildEvents)
            {
                yield return difference;
            }
        }

        private static IEnumerable<ITeamcityEvent> DiffBuild(Build oldBuild, Build newBuild)
        {
            var updatedValues = new List<Expression<Func<Build, object>>>();

            if (newBuild.State != oldBuild.State)
            {
                _log.DebugFormat("Build {0} state has been updated from {1} to {2}", newBuild.Name, oldBuild.State, newBuild.State);

                updatedValues.Add(b => b.State);
            }

            if (false == newBuild.Name.Equals(oldBuild.Name))
            {
                _log.DebugFormat("Build {0} name has been updated from {1} to {2}", newBuild.Name, oldBuild.Name, newBuild.Name);

                updatedValues.Add(b => b.Name);
            }

            if (updatedValues.Count > 0)
            {
                yield return new BuildUpdated(newBuild, updatedValues);
            }
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