using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Face.Web.Utils
{
    public class Base64Converter
    {
        public static string File2String(string filepath)
        {
            var data = System.IO.File.ReadAllBytes(filepath);
            var str = Convert.ToBase64String(data);
            var html = HttpUtility.UrlEncode(str);
            //var c = HttpUtility.UrlEncode(data);
            return html;
        }
    }
}
