using System.Linq;
using HtmlAgilityPack;
using log4net;

namespace Teamcity.Rx.Parser
{
    internal class ProjectRowParser : RowParser
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ProjectRowParser));

        public Project TryParse(HtmlNode[] rowColumns)
        {
            var col = rowColumns.First();
            if (false == col.Attributes["class"].Value.Equals("tcTD_projectName"))
            {
                return null;
            }

            var link = col.SelectSingleNode("div/a");
            var url = link.Attributes["href"].Value;
            var id = GetId(url);
            var name = link.InnerText;

            var project = new Project
            {
                Name = name,
                Url = url,
                Id = id
            };

            _log.DebugFormat("Parsed project {0} ({1})", project.Name, project.Id);

            return project;
        }
    }
}