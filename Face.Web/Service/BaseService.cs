//using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Face.Web.Service
{
    public class BaseService
    {
        /// <summary>
        /// 用来加密APPID和用户名密码
        /// </summary>
        public static String COMMON_PWD
        {
            get;
            set;
        }

        public static String AppID
        {
            get;
            set;
        }

        public static String Cert
        {
            get;
            set;
        }

        public static String SessionID
        {
            get;
            set;
        }

        public static String Password
        {
            get;
            set;
        }

        protected static string serverURI = null;

        //应该尽量保证使用同一个Web, 这样可能
        protected WebHelper web = new WebHelper();

        static BaseService()
        {
            //web.DefaultRequestHeaders.Add("KeepAlive", "true");
        }
    }
}
