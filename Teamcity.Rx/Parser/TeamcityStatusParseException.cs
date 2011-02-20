using System;
using System.Runtime.Serialization;

namespace Teamcity.Rx.Parser
{
    [Serializable]
    public class TeamcityStatusParseException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public TeamcityStatusParseException()
        {
        }

        public TeamcityStatusParseException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }

        public TeamcityStatusParseException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected TeamcityStatusParseException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}