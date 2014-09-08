using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StormPathUserManagement
{
    public class StormpathApplication : StormPathUserManagement.Resource
    {
        public string name { get; set; }

        public string description { get; set; }

        public string status { get; set; }
    }
}