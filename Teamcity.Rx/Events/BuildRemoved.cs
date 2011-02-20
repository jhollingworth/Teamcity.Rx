using System.Runtime.Serialization;

namespace Teamcity.Rx.Events
{
    [DataContract(Name = "BuildRemoved")]
    public class BuildRemoved : ITeamcityEvent
    {
        private readonly string _buildId;
        private readonly string _projectId;

        [DataMember]
        public string EventId
        {
            get { return "rb"; }
        }
        
        [DataMember]
        public string BuildId
        {
            get { return _buildId; }
        }

        [DataMember]
        public string ProjectId
        {
            get { return _projectId; }
        }

        public BuildRemoved(Build build)
        {
            _buildId = build.Id;
            _projectId = build.Project.Id;
        }
    }
}