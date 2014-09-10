using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace StormPathUserManagement
{
    public class DigestAuthenticator : IAuthenticator
    {
        private readonly string _user;
        private readonly string _pass;

        public DigestAuthenticator(string user, string pass)
        {
            _user = user;
            _pass = pass;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            request.Credentials = new NetworkCredential(_user, _pass);

            // TODO: Figure out how to remove the if.. currently PUT does not work if the DigestAuthFixer is in place
            if (request.Method == Method.GET)
            {
                var url = client.BuildUri(request).ToString();
                var uri = new Uri(url);

                var digestAuthFixer = new DigestAuthFixer(client.BaseUrl, _user, _pass);
                digestAuthFixer.GrabResponse(uri.PathAndQuery);
                var digestHeader = digestAuthFixer.GetDigestHeader(uri.PathAndQuery);
                request.AddParameter("Authorization", digestHeader, ParameterType.HttpHeader);
            }

        }
    }
}
