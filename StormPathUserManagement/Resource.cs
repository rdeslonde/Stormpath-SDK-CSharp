using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StormPathUserManagement
{
    /**
     * This class is the base for all Stormpath resource wrappers.
     */
    public abstract class Resource
    {
        public string href { get; set; }
        public string content { get; set; }

    }
}
