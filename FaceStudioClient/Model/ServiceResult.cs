using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudioClient.Model
{
    public class ServiceResult<T>
    {
        public int result
        {
            get;set;
        }

        public Boolean success
        {
            get;set;
        }

        public string msg
        {
            get; set;
        }

        public T data
        {
            get;set;
        }
    }
}
