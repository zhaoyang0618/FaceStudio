using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.Contract
{
    public class CheckinRecord
    {
        public int ID
        {
            get;set;
        }

        /// <summary>
        /// 设备IP
        /// </summary>
        public string ip
        {
            get;set;
        }

        public string deviceKey
        {
            get;set;
        }

        public string personId
        {
            get;set;
        }

        public string time
        {
            get;set;
        }

        public string type
        {
            get;set;
        }

        public string path
        {
            get;set;
        }
    }
}
