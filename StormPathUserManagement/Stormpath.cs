using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace StormPathUserManagement
{
    public class Stormpath : IStormpath
    {
        public enum ExpansionLinkTypes
        {
            account, groups, groupmemberships, directory, tenant, customData
        }   

        private StormathClient _client;
        private StormpathApplication _application;
        private string _accountshref;

        public Stormpath(string apiKeyId, string apiKeySecret, string applicationHref, string accountshref, AuthenticationType authenticationType)
        {
            _client = new StormathClient(apiKeyId, apiKeySecret, authenticationType);
            _application = new StormpathApplication()
            {
                href = applicationHref
            };

            _accountshref = accountshref;
        }

        public StormpathAccount<T> CreateAccount<T>(string email, string password, string fullName, string givenName, string surName, string status)
        {
            var account = new StormpathAccount<T>
            {
                email = email,
                fullName = fullName,
                givenName = givenName,
                surname = surName,
                password = password,
                status = status
            };

            var request = new RestRequest(Method.POST)
            {
                Resource = "/accounts", 
                RequestFormat = DataFormat.Json
            };

            request.AddBody(account);

            var response = _client.Execute<StormpathAccount<T>>(request, _application.href);
            response.Resource.content = response.Response;

            return response.Resource;
        }

        public StormpathAccount<T> CreateAccount<T>(string email, string password, string fullName, string givenName, string surName, string status, dynamic customData, List<ExpansionLinkTypes> expansionLinks)
        {
            var account = new
            {
                email = email,
                fullName = fullName,
                givenName = givenName,
                surname = surName,
                password = password,
                status = status,
                customData = customData
            };

            var request = new RestRequest(Method.POST)
            {
                //we want the custom data back too
                Resource = "/accounts" + GetExpansionLinkTypes(expansionLinks),
                RequestFormat = DataFormat.Json
            };

            request.AddBody(account);

            var response = _client.Execute<StormpathAccount<T>>(request, _application.href);
            response.Resource.content = response.Response;

            return response.Resource;
        }

        public StormpathAccount<T> UpdateAccount<T>(string accountGuid, string email, string password, string fullName, string givenName, string surName, string status, List<ExpansionLinkTypes> expansionLinks)
        {
            /*  //can't use this because restsharp doesn't support conditional serializing
            var account = new StormpathAccount<T>()
            {
                email = email,
                fullName = fullName,
                givenName = givenName,
                surname = surName,
                password = password,
                status = status
            };
            */

            //this is the only way to get conditional serializing with restsharp...an expando object.
            dynamic account = new ExpandoObject();
            if (!string.IsNullOrEmpty(email))
            {
                account.email = email;
            }

            if (!string.IsNullOrEmpty(password))
            {
                account.password = password;
            }

            if (!string.IsNullOrEmpty(email))
            {
                account.email = email;
            }

            if (!string.IsNullOrEmpty(fullName))
            {
                account.fullname = fullName;
            }

            if (!string.IsNullOrEmpty(givenName))
            {
                account.givenName = givenName;
            }

            if (!string.IsNullOrEmpty(surName))
            {
                account.surname = surName;
            }

            if (!string.IsNullOrEmpty(status))
            {
                account.status = status;
            }

            var request = new RestRequest(Method.POST)
            {
                Resource = accountGuid + GetExpansionLinkTypes(expansionLinks),
                RequestFormat = DataFormat.Json
            };

            request.AddBody(account);

            var response = _client.Execute<StormpathAccount<T>>(request, _accountshref);
            response.Resource.content = response.Response;

            return response.Resource;
        }


        public StormpathAccount<T> RetrieveAccount<T>(string accountGuid, List<Stormpath.ExpansionLinkTypes> expansionLinks)
        {
            expansionLinks.Remove(ExpansionLinkTypes.account); //account is not a supported link expansion for retrieve
            expansionLinks.Remove(ExpansionLinkTypes.groupmemberships); //groupMemberships is not a supported link expansion for retrieve

            var request = new RestRequest(Method.GET)
            {
                Resource = accountGuid + GetExpansionLinkTypes(expansionLinks),
                RequestFormat = DataFormat.Json
            };

            var response = _client.Execute<StormpathAccount<T>>(request, _accountshref);
            response.Resource.content = response.Response;

            return response.Resource;
        }

        public StormpathAccount<T> AuthenticateAccount<T>(string authEmail, string authPassword, List<Stormpath.ExpansionLinkTypes> expansionLinks)
        {
            expansionLinks.Remove(ExpansionLinkTypes.customData); //customData is not a supported link expansion for retrieve
            expansionLinks.Remove(ExpansionLinkTypes.tenant); //tenant is not a supported link expansion for retrieve
            expansionLinks.Remove(ExpansionLinkTypes.directory); //directory is not a supported link expansion for retrieve
            expansionLinks.Remove(ExpansionLinkTypes.groups); //groups is not a supported link expansion for retrieve
            expansionLinks.Remove(ExpansionLinkTypes.groupmemberships); //groupmemberships is not a supported link expansion for retrieve

            var accountToAttemptLogin = new StormpathAccount<T>() { email = authEmail, password = authPassword };

            var loginAttempt = new BasicLoginAttempt();
            var authStr = accountToAttemptLogin.email + ":" + accountToAttemptLogin.password;

            //Username and password need to be base64 encoded before they are sent to Stormpath
            loginAttempt.value = Convert.ToBase64String(Encoding.UTF8.GetBytes(authStr));

            var request = new RestRequest(Method.POST)
            {
                Resource = "/loginAttempts" + GetExpansionLinkTypes(expansionLinks),
                RequestFormat = DataFormat.Json
            };

            request.AddBody(loginAttempt);
            var authResult = _client.Execute<AuthenticationResult<T>>(request, _application.href);
            authResult.Resource.content = authResult.Response;

            return authResult.Resource.account;
        }

        public bool DeleteAccount<T>(string accountGuid)
        {
            var request = new RestRequest(Method.DELETE)
            {
                Resource = accountGuid,
                RequestFormat = DataFormat.Json
            };

            var deleteResult = _client.Execute<AuthenticationResult<T>>(request, _accountshref);

            return (deleteResult.Response == "");
        }

        private string GetExpansionLinkTypes(List<Stormpath.ExpansionLinkTypes> expansionLinks)
        {
            string expansionLinksString = string.Join(",", expansionLinks);

            return (!string.IsNullOrEmpty(expansionLinksString) ? "?expand=" + expansionLinksString : "");
        }
    }
}
