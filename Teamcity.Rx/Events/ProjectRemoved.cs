using System.Runtime.Serialization;

namespace Teamcity.Rx.Events
{
    [DataContract(Name = "ProjectRemoved")]
    public class ProjectRemoved : ITeamcityEvent
    {
        [DataMember]
        public string ProjectId { get; private set; }

        [DataMember]
        public string EventId
        {
            get { return "rp"; }
        }

        public ProjectRemoved(Project project)
        {
            ProjectId = project.Id;
        }
    }
}