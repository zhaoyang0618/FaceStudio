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

        public void Add(CheckinRecord[] records)
        {
            //首先需要保证不存在:这里需要事务处理
            int count = 0;
            foreach(var rec in records)
            {
                var ret = Get(x => x.PersonId == rec.PersonId && x.Time == rec.Time && x.IP == rec.IP);
                if (ret == null || ret.Count() == 0)
                {
                    Insert(rec);
                    count++;
                }
            }
            if(count > 0)
            {
                context.SaveChanges();
            }
        }
    }
}
