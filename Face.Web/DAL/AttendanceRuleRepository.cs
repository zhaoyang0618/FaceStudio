using Face.Contract;
using Face.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Face.Web.DAL
{
    /// <summary>
    /// 考勤规则
    /// </summary>
    public class AttendanceRuleRepository : GenericRepository<AttendanceRule>
    {
        public AttendanceRuleRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
