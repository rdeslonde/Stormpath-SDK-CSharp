using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StormPathUserManagement
{
    internal class APIResult<T>
    {
        T resource;

        internal T Resource
        {
            get { return resource; }
            set { resource = value; }
        }
        string result;

        internal string Response
        {
            get { return result; }
            set { result = value; }
        }






    }
}
