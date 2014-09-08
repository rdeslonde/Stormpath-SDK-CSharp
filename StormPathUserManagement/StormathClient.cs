using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using RestSharp.Deserializers;

namespace StormPathUserManagement
{
    internal class StormathClient
    {
        const string BaseUrl = "https://api.stormpath.com/v1";

        readonly string _apiKeyId;
        readonly string _apiKeySecret;

        internal StormathClient(string apiKeyId, string apiKeySecret)
        {
            _apiKeyId = apiKeyId;
            _apiKeySecret = apiKeySecret;
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
                Authenticator = new HttpBasicAuthenticator(_apiKeyId, _apiKeySecret)
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
