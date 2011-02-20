using System.Text.RegularExpressions;
using HtmlAgilityPack;
using log4net;

namespace Teamcity.Rx.Parser
{
    internal class BuildRowParser : RowParser
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(BuildRowParser));

        public Build Parse(Project project, HtmlNode[] columns)
        {
            _log.DebugFormat("Parsing a build configuration for {0}", project.Name);

            if (columns.Length != 3)
            {
                throw new TeamcityStatusParseException("The build row had the incorrect number of columns");
            }

            var buildConfigurationColumn = columns[0];
            var buildConfigurationLink = buildConfigurationColumn.SelectSingleNode("a");
            var buildConfigurationUrl = buildConfigurationLink.Attributes["href"].Value;
            var currentBuildLink = columns[1].SelectSingleNode("div/a");

            var build = new Build
            {
                Project = project,
                Name = buildConfigurationLink.InnerText,
                Id = GetId(buildConfigurationUrl),
                State = GetBuildState(buildConfigurationColumn),
                Url = buildConfigurationUrl
            };

            if (currentBuildLink != null)
            {
                build.CurrentBuildNumber = currentBuildLink.InnerText.Replace("#", string.Empty);
                build.CurrentBuildUrl = currentBuildLink.Attributes["href"].Value;
            }

            _log.DebugFormat("Parsed build configuration {0} ({1}) - {2}", build.Name, build.Id, build.State);

            return build;
        }

        private static BuildState GetBuildState(HtmlNode buildConfigurationColumn)
        {
            var result = Regex.Match(buildConfigurationColumn.SelectSingleNode("img").Attributes["src"].Value, @"buildStates/(.*)\.gif");

            switch (result.Groups[1].Value.ToLower())
            {
                case "success":
                    return BuildState.Success;
                case "error":
                    return BuildState.Failed;
                default:
                    return BuildState.Unkown;
            }
        }
    }
}