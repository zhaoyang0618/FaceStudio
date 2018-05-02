using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.Contract
{
    public class Department
    {
        public Guid ID
        {
            get;set;
        }

        public Guid? ParentDepartmentID { get; set; }

        /// <summary>
        /// 部门有层级关系
        /// </summary>
        [ForeignKey("ParentDepartmentID")]
        public virtual Department ParentDepartment
        {
            get;set;
        }

        [MaxLength(100)]
        public string Name
        {
            get;set;
        }

        public string Description
        {
            get;set;
        }

        public DateTime CreateTime
        {
            get;set;
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

        public Department()
        {
            this.CreateTime = DateTime.Now;
            this.UpdateTime = DateTime.Now;
        }
    }
}
