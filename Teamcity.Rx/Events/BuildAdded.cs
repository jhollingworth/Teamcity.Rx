using System.Runtime.Serialization;

namespace Teamcity.Rx.Events
{
    [DataContract(Name = "AddedBuild")]
    public class BuildAdded : ITeamcityEvent
    {
        private readonly Build _build;

        [DataMember]
        public string EventId
        {
            get { return "ab"; }
        }

        [DataMember]
        public Build Build
        {
            get { return _build; }
        }

        public BuildAdded(Build build)
        {
            _build = build;
        }
    }
}