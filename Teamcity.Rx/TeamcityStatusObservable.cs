using System;
using System.Net;
using System.Threading;
using Hammock;
using Hammock.Authentication;
using Hammock.Authentication.Basic;

namespace Teamcity.Rx
{
    internal class TeamcityStatusObservable : BackgroundWorkerObservable<string>
    {
        private readonly IWebCredentials _credentials;
        private readonly IRestClient _client;

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
                var response = _client.Request(new RestRequest { Credentials = _credentials, Path = "externalStatus.html" });

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    OnError(new Exception(string.Format("{0} returned while trying to get the external status", response.StatusCode)));
                }

                return response.Content;
            }
            catch (Exception ex)
            {
                OnError(ex);

                return null;
            }
        }
    }
}