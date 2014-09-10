using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using RestSharp.Deserializers;

namespace StormPathUserManagement
{
    public enum AuthenticationType
    {
        basic, digest
    }

    internal class StormathClient
    {
        private readonly int _merchantId;

        private AuthenticationType _authenticationType;


        const string BaseUrl = "https://api.stormpath.com/v1";

        readonly string _apiKeyId;
        readonly string _apiKeySecret;

        internal StormathClient(string apiKeyId, string apiKeySecret, AuthenticationType authenticationType)
        {
            _apiKeyId = apiKeyId;
            _apiKeySecret = apiKeySecret;
            _authenticationType = authenticationType;
        }

        internal APIResult<T> Execute<T>(RestRequest request) where T : new()
        {
            return Execute<T>(request, BaseUrl);
        }

        internal APIResult<T> Execute<T>(RestRequest request, string href) where T : new()
        {
            //request.JsonSerializer = new RestSharpJsonNetSerializer();

            var client = new RestClient
            {
                BaseUrl = href,
                Authenticator = new DigestAuthenticator(_apiKeyId, _apiKeySecret)//new HttpBasicAuthenticator(_apiKeyId, _apiKeySecret)
            };

            var response = client.Execute<T>(request);

            int httpStatus = (int)(response.StatusCode);
            
            if (httpStatus >= 400)
            {
                // Converting the REST API error into an exception with all the properties from the Json body.
                var requestException = new StormpathException(JsonConvert.DeserializeObject<StormpathError>(response.Content));
                throw requestException;
            }

            var result = new APIResult<T>
            {
                Resource = response.Data, 
                Response = response.Content
            };

            return result;
        }

    }
}
