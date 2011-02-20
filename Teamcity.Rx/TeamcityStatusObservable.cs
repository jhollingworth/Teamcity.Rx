using System;
using System.Net;
using System.Threading;
using Hammock;
using Hammock.Authentication;
using Hammock.Authentication.Basic;
using log4net;

namespace Teamcity.Rx
{
    internal class TeamcityStatusObservable : BackgroundWorkerObservable<string>
    {
        const string ExternalstatusFileName = "externalStatus.html";

        private readonly IWebCredentials _credentials;
        private readonly RestClient _client;

        private static readonly ILog _log = LogManager.GetLogger(typeof (TeamcityStatusObservable));

        public TeamcityStatusObservable(string teamcityUrl, string username, string password)
        {
            _credentials = new BasicAuthCredentials {Username = username, Password = password};
            _client = new RestClient {Authority = teamcityUrl};
            
            Start();
        }

        protected override string GetObservableEvents()
        {
            try
            {
                _log.DebugFormat("Trying to get teamcity status from {0}/{1}", _client.Authority, ExternalstatusFileName);

                var response = _client.Request(new RestRequest { Credentials = _credentials, Path = ExternalstatusFileName });

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    _log.WarnFormat("Failed to get teamcity status. Teamcity responded with {0}", response.StatusCode);

                    OnError(new Exception(string.Format("{0} returned while trying to get the external status", response.StatusCode)));
                }

                return response.Content;
            }
            catch (Exception ex)
            {
                _log.Warn("Failed getting teamcity status", ex);

                OnError(ex);

                return null;
            }
        }
    }
}