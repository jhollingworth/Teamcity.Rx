using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Machine.Specifications;
using Teamcity.Rx.Parser;

namespace Teamcity.Rx.Specs
{
    public class TeamcityStatusParserSpecs
    {
        static string Html;
        protected static List<Project> Projects;
        protected static TeamcityStatusParser Subject;
        
        protected static string TestDataFile
        {
            set
            {
                Html = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "test_data", value + ".html"));
            }
        }
        
        protected static void parsed_the_html()
        {
            new TeamcityStatusParser(Html).Subscribe(projects => Projects = projects);
        }

        protected static Project the_project(string name)
        {
            return Projects.SingleOrDefault(p => p.Name.Equals(name));
        }
    }
    
    [Subject(typeof(TeamcityStatusParser))]
    public class When_I_try_to_parse_the_teamcity_status_html : TeamcityStatusParserSpecs
    {
        Establish context = () => TestDataFile = "test1";
        Because I = parsed_the_html;
        It should_return_a_project = () => the_project("Project A").ShouldNotBeNull();
        It should_find_all_of_the_builds = () => the_project("Project A").Builds.Length.ShouldEqual(4);
        It should_find_failed_builds = () => the_project("Website").Builds.First(b => b.Name.Equals("Site")).State.ShouldEqual(BuildState.Failed);
    }
}
