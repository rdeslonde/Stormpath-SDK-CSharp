using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StormPathUserManagement
{
    /**
     * This class is a wrapper for the Stormpath account resource.
     */
    public class StormpathAccount<T> : Resource
    {
        public string givenName { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string fullName { get; set; }
        public string middleName { get; set; }
        public string status { get; set; }
        public StormpathDirectory Directory { get; set; }
        public StormpathGroups Groups { get; set; }
        public StormpathGroups GroupMemberships { get; set; }
        public StormpathTenant<dynamic> Tenant { get; set; }
        public T customData { get; set; }
    }
}
