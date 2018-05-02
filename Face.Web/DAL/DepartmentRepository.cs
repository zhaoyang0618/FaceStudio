using Face.Contract;
using Face.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Face.Web.DAL
{
    public class DepartmentRepository : GenericRepository<Department>
    {
        public DepartmentRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
