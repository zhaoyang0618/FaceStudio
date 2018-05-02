using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.Contract
{
    public class SpecialAttendanceRule
    {
        public Guid ID
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        /// <summary>
        /// 上班时间
        /// </summary>
        public TimeSpan? StartTime
        {
            get; set;
        }

        /// <summary>
        /// 上班时间
        /// </summary>
        public TimeSpan? EndTime
        {
            get; set;
        }

        /// <summary>
        /// 上班时间
        /// </summary>
        public DateTime StartDate
        {
            get; set;
        }

        /// <summary>
        /// 上班时间
        /// </summary>
        public DateTime EndDate
        {
            get; set;
        }

        /// <summary>
        /// 0-工作日;1-休息日
        /// </summary>
        public int DateType
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

        public SpecialAttendanceRule()
        {
            this.CreateTime = DateTime.Now;
            this.UpdateTime = DateTime.Now;
        }
    }
}
