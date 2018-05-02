using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.Contract
{
    public class ServiceResult<T> : ServiceBaseResult
    {
        public T data
        {
            get; set;
        }
    }
}
