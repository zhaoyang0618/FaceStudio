using Face.Contract;
using Face.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Face.Web.DAL
{
    public class SpecialAttendanceRuleRepository : GenericRepository<SpecialAttendanceRule>
    {
        public SpecialAttendanceRuleRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}