using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.Contract
{
    public class CheckinRecord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID
        {
            get;set;
        }

        /// <summary>
        /// 设备IP
        /// </summary>
        public string IP
        {
            get;set;
        }

        public string DeviceKey
        {
            get;set;
        }

        /// <summary>
        /// 设备内建的ID
        /// </summary>
        public string InnerID
        {
            get;set;
        }

        public string PersonId
        {
            get;set;
        }

        public Guid? EmployeeID
        {
            get;set;
        }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee
        {
            get;set;
        }

        public long Time
        {
            get;set;
        }

        /// <summary>
        /// 这个数据从Time转化得到
        /// </summary>
        public DateTime CheckinTime
        {
            get;set;
        }

        public int State
        {
            get;set;
        }

        public int Type
        {
            get;set;
        }

        public string Path
        {
            get;set;
        }

        /// <summary>
        /// 可能需要保存到本地,此时需要本地路径
        /// </summary>
        public string LocalPath
        {
            get;set;
        }

        public DateTime CreateTime
        {
            get;set;
        }

        public CheckinRecord()
        {
            this.CreateTime = DateTime.Now;
        }
    }
}
