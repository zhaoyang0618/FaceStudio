using Face.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Face.Web.Models
{
    public class PhotoImageQueryItem
    {
        /// <summary>
        /// 向哪个相机查询相关数据:主要是IP;Port;Pwd
        /// </summary>
        public Camera Camera
        {
            get;set;
        }

        public string PersonID
        {
            get;set;
        }

        public string FaceID
        {
            get;set;
        }

        public Guid PhotoImageID
        {
            get;set;
        }
    }
}