using System.Linq;
using HtmlAgilityPack;

namespace Teamcity.Rx.Parser
{
    internal class ProjectRowParser : RowParser
    {
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

            return new Project
            {
                Name = name,
                Url = url,
                Id = id
            };
        }
    }
}