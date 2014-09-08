using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StormPathUserManagement
{
    /**
     * This class serves a the wrapper for an account login attempt.
     */
    public class BasicLoginAttempt : Resource
    {
        public BasicLoginAttempt()
        {
            this.type = "basic";
        }
        public string type { get; set; }
        public string value { get; set; }
    }
}
