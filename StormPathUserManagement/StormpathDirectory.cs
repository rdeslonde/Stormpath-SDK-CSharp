using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StormPathUserManagement
{
    public class StormpathDirectory : Resource
    {
        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string tenant { get; set; }
    }
}
