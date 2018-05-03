using Face.Contract;
using Face.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Face.Web.DAL
{
    public class CheckinRecordRepository : GenericRepository<CheckinRecord>
    {
        public CheckinRecordRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public List<CheckinRecord> Query(CheckinRecordQueryInfo query)
        {
            var theses = from a in dbSet
                         select a;
            if (!String.IsNullOrEmpty(query.EmployeeName))
            {
                theses = theses.Where(a => a.Employee != null && a.Employee.Name.Equals(query.EmployeeName, StringComparison.OrdinalIgnoreCase));
            }

            if (query.StartDate != null)
            {
                theses = theses.Where(a => a.CheckinTime >= query.StartDate.Value);
            }

            if (query.EndDate != null)
            {
                theses = theses.Where(a => a.CheckinTime < query.EndDate.Value);
            }

            theses.OrderByDescending(t => t.ID);
            return theses.ToList();
        }
    }
}
