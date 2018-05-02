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
    /// 对应关系
    /// </summary>
    public class EmployeeCameraRelation
    {
        [Key]
        [Column(Order = 0)]
        public Guid EmployeeID
        {
            get;set;
        }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee
        {
            get;set;
        }

        [Key]
        [Column(Order = 1)]
        public Guid CameraID
        {
            get;set;
        }

        [ForeignKey("CameraID")]
        public virtual Camera Camera
        {
            get;set;
        }

        public int Status
        {
            get;set;
        }

        /// <summary>
        /// 员工在该设备上的ID:应该和就是工号
        /// </summary>
        public string PersonID
        {
            get;set;
        }

        /// <summary>
        /// 员工照片在该设备上的ID:应该和就是工号
        /// </summary>
        public string FirstPhotoID
        {
            get;set;
        }

        /// <summary>
        /// 员工照片在该设备上的ID:应该和就是工号
        /// </summary>
        public string SecondPhotoID
        {
            get; set;
        }

        /// <summary>
        /// 员工照片在该设备上的ID:应该和就是工号
        /// </summary>
        public string ThirdPhotoID
        {
            get; set;
        }
    }
}

