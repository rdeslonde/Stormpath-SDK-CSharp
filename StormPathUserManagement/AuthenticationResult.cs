using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StormPathUserManagement
{
    /**
     * This class is a wrapper for the authentication result after a login attempt.
     */
    public class AuthenticationResult<T> : Resource
    {
        public StormpathAccount<T> account { get; set; }
    }
}
