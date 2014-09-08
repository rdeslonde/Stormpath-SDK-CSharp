using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StormPathUserManagement
{
    interface IStormpath
    {

        StormpathAccount<T> CreateAccount<T>(string email, string password, string fullName, string givenName, string surName, string status, dynamic customData, List<Stormpath.ExpansionLinkTypes> expansionLinks);

        StormpathAccount<T> AuthenticateAccount<T>(string authEmail, string authPassword, List<Stormpath.ExpansionLinkTypes> expansionLinks);

        StormpathAccount<T> RetrieveAccount<T>(string accountGuid, List<Stormpath.ExpansionLinkTypes> expansionLinks);

        StormpathAccount<T> CreateAccount<T>(string email, string password, string fullName, string givenName, string surName, string status);

        StormpathAccount<T> UpdateAccount<T>(string accountGuid, string email, string password, string fullName, string givenName, string surName, string status, List<Stormpath.ExpansionLinkTypes> expansionLinks);
        
        bool DeleteAccount<T>(string accountGuid);
    }
}
