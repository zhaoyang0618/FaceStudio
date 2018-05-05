using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.Contract
{
    public class ServiceBaseResult
    {
        public int result
        {
            get;set;
        }

        public bool success
        {
            get;set;
        }

        public string msg
        {
            get;set;
        }
    }
}
