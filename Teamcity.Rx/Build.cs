using System.Diagnostics;
using System.Runtime.Serialization;

namespace Teamcity.Rx
{
    [DataContract(Name = "Build")]
    [DebuggerDisplay("{Name} ({Id}) - {State}")]
    public class Build 
    {
        [DataMember]
        public string Id { get; set; }
        
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public BuildState State { get; set; }

        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public string CurrentBuildNumber { get; set; }

        [DataMember]
        public string CurrentBuildUrl { get; set; }

        public Project Project { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof (Build) && Equals((Build) obj);
        }

        public bool Equals(Build other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || Equals(other.Id, Id);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }

        public static bool operator ==(Build left, Build right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Build left, Build right)
        {
            return false == Equals(left, right);
        }
        
        internal Build()
        {
        }
    }
}