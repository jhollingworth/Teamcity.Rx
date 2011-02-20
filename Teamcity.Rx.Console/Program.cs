using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using log4net.Config;

namespace Teamcity.Rx.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            var teamcityObservable = new TeamcityObservable(ConfigurationManager.AppSettings["hostname"], ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]);
            teamcityObservable.Subscribe(e => System.Console.WriteLine(e.EventId));
            
            System.Console.ReadKey();
        }
    }
}
