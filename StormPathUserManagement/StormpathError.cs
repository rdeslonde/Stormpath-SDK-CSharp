using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StormPathUserManagement
{
    public class StormpathError : Resource
    {
        public int code { get; set; }
        public int status { get; set; }
        public string message { get; set; }
        public string developerMessage { get; set; }
        public string moreInfo { get; set; }
    }
}
