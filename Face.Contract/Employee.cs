using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.Contract
{
    /// <summary>
    /// 人员
    /// </summary>
    public class Employee
    {
        public Guid ID
        {
            get;set;
        }

        [MaxLength(100)]
        public string Name
        {
            get;set;
        }

        /// <summary>
        /// 工号
        /// </summary>
        [MaxLength(100)]
        public string Code
        {
            get;set;
        }

        [MaxLength(100)]
        public string IDCard
        {
            get;set;
        }

        [MaxLength(100)]
        public string Position
        {
            get;set;
        }

        //public Guid? DeparmentID
        //{
        //    get;set;
        //}

        //[ForeignKey("DeparmentID")]
        public virtual Department Deparment
        {
            get;set;
        }

        public string Mobile
        {
            get;set;
        }

        /// <summary>
        /// 性别:0-未知；1-男性；2-女性
        /// </summary>
        public int Sex
        {
            get;set;
        }

        //public Guid? FirstPhotoID
        //{
        //    get; set;
        //}

        //[ForeignKey("FirstPhotoID")]
        public virtual PhotoImage FirstPhoto
        {
            get;set;
        }

        //public Guid? SecondPhotoID
        //{
        //    get; set;
        //}

        //[ForeignKey("SecondPhotoID")]
        public virtual PhotoImage SecondPhoto
        {
            get; set;
        }

        //public Guid? ThirdPhotoID
        //{
        //    get; set;
        //}

        //[ForeignKey("ThirdPhotoID")]
        public virtual PhotoImage ThirdPhoto
        {
            get; set;
        }

        /// <summary>
        /// 0-表示尚未处理;1-表示至少在某一设备创建;-1表示没有在任何设备创建
        /// </summary>
        public int Status
        {
            get;set;
        }

        /// <summary>
        /// 考勤规则
        /// </summary>
        public virtual AttendanceRule AttendanceRule
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

        public virtual List<EmployeeCameraRelation> Cameras
        {
            get;set;
        }

        public Employee()
        {
            this.Cameras = new List<EmployeeCameraRelation>();
            this.CreateTime = DateTime.Now;
            this.UpdateTime = DateTime.Now;
        }
    }
}
