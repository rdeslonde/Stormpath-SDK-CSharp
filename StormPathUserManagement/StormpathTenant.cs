using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StormPathUserManagement
{
    public class StormpathTenant<T> : Resource
    {
        public string name { get; set; }
        public string key { get; set; }
        public T customData { get; set; }

    }
}
