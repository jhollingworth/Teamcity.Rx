using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Teamcity.Rx
{
    [DebuggerDisplay("{_projects.Values.Count} Projects")]
    public class Teamcity : IEnumerable<Project>
    {
        private readonly object _projectsLock = new object();
        private readonly Dictionary<string, Project> _projects = new Dictionary<string, Project>();

        public bool Loaded { get; private set; }

        public Project this[string projectId]
        {
            get
            {
                lock (_projectsLock)
                {
                    Project project;

                    return _projects.TryGetValue(projectId, out project) ? project : null;
                }
            }
        }
        
        internal void UpdateProjects(IEnumerable<Project> projects)
        {
            lock (_projectsLock)
            {
                Loaded = true;
                
                _projects.Clear();
                
                foreach (var project in projects)
                {
                    _projects[project.Name] = project;
                }
            }
        }

        internal Teamcity()
        {
        }

        public IEnumerator<Project> GetEnumerator()
        {
            lock (_projectsLock)
            {
                return _projects.Values.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
