using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.Contract
{
    public class AttendanceRule
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
        public TimeSpan StartTime
        {
            get; set;
        }

        /// <summary>
        /// 上班时间
        /// </summary>
        public TimeSpan EndTime
        {
            get; set;
        }

        /// <summary>
        /// 周一是否计入考勤
        /// </summary>
        public bool Monday
        {
            get; set;
        }

        public bool Tuesday
        {
            get;set;
        }

        public bool Wednesday
        {
            get;set;
        }

        public bool Thursday
        {
            get;set;
        }

        public bool Friday
        {
            get;set;
        }

        public bool Saturday
        {
            get;set;
        }

        public bool Sunday
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

        public AttendanceRule()
        {
            this.StartTime = new TimeSpan(8, 0, 0);
            this.EndTime = new TimeSpan(17, 0, 0);
            this.Monday = true;
            this.Tuesday = true;
            this.Wednesday = true;
            this.Thursday = true;
            this.Friday = true;
            this.CreateTime = DateTime.Now;
            this.UpdateTime = DateTime.Now;
        }
    }
}
