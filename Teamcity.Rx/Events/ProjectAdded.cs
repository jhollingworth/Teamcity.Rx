using System.Runtime.Serialization;

namespace Teamcity.Rx.Events
{
    [DataContract(Name = "AddedProject")]
    public class ProjectAdded : ITeamcityEvent
    {
        private readonly Project _project;

        [DataMember]
        public Project Project
        {
            get { return _project; }
        }

        [DataMember]
        public string EventId
        {
            get { return "ap"; }
        }

        public ProjectAdded(Project project)
        {
            _project = project;
        }
    }
}