using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.Contract
{
    public class PhotoImage
    {
        public Guid ID
        {
            get;set;
        }

        [MaxLength(1024)]
        public string FileName
        {
            get;set;
        }

        [MaxLength(1024)]
        public string FilePath
        {
            get;set;
        }

        [MaxLength(1024)]
        public string BaseFolder
        {
            get;set;
        }

        [MaxLength(1024)]
        public string MimeType
        {
            get;set;
        }

        public string Feature
        {
            get;set;
        }

        public string FeatureKey
        {
            get;set;
        }

        /// <summary>
        /// 0-表示尚未处理;1-表示成功;-1表示不能使用
        /// </summary>
        public int Status
        {
            get;set;
        }

        public DateTime CreateTime
        {
            get; set;
        }

        [MaxLength(100)]
        public string CreateUser
        {
            get; set;
        }

        public DateTime UpdateTime
        {
            get; set;
        }

        [MaxLength(100)]
        public string UpdateUser
        {
            get; set;
        }

        public PhotoImage()
        {
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
        }
    }
}
