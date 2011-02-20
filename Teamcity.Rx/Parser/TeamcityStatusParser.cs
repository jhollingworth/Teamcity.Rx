using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using log4net;

namespace Teamcity.Rx.Parser
{
    public class TeamcityStatusParser : IObservable<List<Project>>
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(TeamcityStatusParser));
        private readonly List<Project> _projects;

        public TeamcityStatusParser(string html)
        {
            _log.Debug("Parsing the teamcity status html");

            _projects = Parse(html);
        }

        private static List<Project> Parse(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var projects = new List<Project>();
            var projectParser = new ProjectRowParser();
            var buildRowParser = new BuildRowParser();

            Project currentProject = null;

            foreach (var row in doc.DocumentNode.SelectNodes("//tr"))
            {
                var columns = row.SelectNodes("td").ToArray();

                if (columns.Count() < 1)
                {
                    continue;
                }

                var project = projectParser.TryParse(columns);

                if (null != project)
                {
                    projects.Add(project);
                    currentProject = project;
                }
                else
                {
                    if (null == currentProject)
                    {
                        throw new TeamcityStatusParseException("Trying to parse a build but there is no project associated with it", new NullReferenceException());
                    }

                    currentProject.AddBuild(buildRowParser.Parse(currentProject, columns));
                }
            }

            return projects;
        }

        public IDisposable Subscribe(IObserver<List<Project>> observer)
        {
            observer.OnNext(_projects);
            observer.OnCompleted();

            return new EmptyDisposable();
        }
    }
}