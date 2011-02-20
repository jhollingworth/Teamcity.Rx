using System.Text.RegularExpressions;

namespace Teamcity.Rx.Parser
{
    internal abstract class RowParser
    {
        protected string GetId(string url)
        {
            var match = Regex.Match(url, @"Id=(.*)\&");

            if (false == match.Success || match.Groups.Count != 2)
            {
                throw new TeamcityStatusParseException("Could not extract the id from the url {0}", url);
            }

            return match.Groups[1].Value;
        }
    }
}