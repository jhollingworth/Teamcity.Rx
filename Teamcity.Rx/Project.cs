using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using Teamcity.Rx.Events;

namespace Teamcity.Rx
{
    [DataContract(Name = "Project")]
    [DebuggerDisplay("{Name} ({Id}) - {Builds.Length} Builds")]
    public class Project : IEnumerable<Build>
    {
        private readonly Dictionary<string, Build> _builds = new Dictionary<string, Build>();

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public Build[] Builds
        {
            get { return this.ToArray(); }
        }
        
        internal Project()
        {
        }

        public void AddBuild(Build build)
        {
            _builds[build.Name] = build;
        }

        public Build this[string buildName]
        {
            get
            {
                Build build;
                
                return _builds.TryGetValue(buildName, out build) ? build : null;
            }
        }

        public IEnumerator<Build> GetEnumerator()
        {
            return _builds.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Project)) return false;
            return Equals((Project) obj);
        }

        public bool Equals(Project other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }

        public static bool operator ==(Project left, Project right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Project left, Project right)
        {
            return !Equals(left, right);
        }
    }
}
