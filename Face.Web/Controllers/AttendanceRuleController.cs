using Face.Contract;
using Face.Web.DAL;
using Face.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;

namespace Face.Web.Controllers
{
    [RoutePrefix("api/AttendanceRule")]
    public class AttendanceRuleController : ApiController
    {
        // GET: AttendanceRule
        //public ActionResult Index()
        //{
        //    return View();
        //}
        internal ApplicationDbContext db = new ApplicationDbContext();

        [Route("Save")]
        public AttendanceRule Save(AttendanceRule entity)
        {
            if (entity == null)
                return null;

            try
            {
                var rep = new AttendanceRuleRepository(db);
                if (entity.ID == Guid.Empty)
                {
                    entity.ID = Guid.NewGuid();
                    entity.CreateUser = HttpContext.Current.User.Identity.Name;
                    entity.UpdateUser = HttpContext.Current.User.Identity.Name;
                    rep.Insert(entity);
                    db.SaveChanges();
                }
                else
                {
                    entity.UpdateTime = DateTime.Now;
                    entity.UpdateUser = HttpContext.Current.User.Identity.Name;
                    rep.Update(entity);
                    db.SaveChanges();
                }

                return entity;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [Route("Query")]
        public IEnumerable<AttendanceRule> Query()
        {
            var rep = new AttendanceRuleRepository(db);
            return rep.Get(null, x => x.OrderBy(y => y.CreateTime));
        }

        [Route("Delete")]
        public AttendanceRule Delete(AttendanceRule entiry)
        {
            var rep = new AttendanceRuleRepository(db);
            rep.Delete(entiry);
            db.SaveChanges();
            return entiry;
        }

        [Route("SaveSpecial")]
        public SpecialAttendanceRule SaveSpecial(SpecialAttendanceRule entity)
        {
            if (entity == null)
                return null;

            try
            {
                var rep = new SpecialAttendanceRuleRepository(db);
                if (entity.ID == Guid.Empty)
                {
                    entity.ID = Guid.NewGuid();
                    entity.CreateUser = HttpContext.Current.User.Identity.Name;
                    entity.UpdateUser = HttpContext.Current.User.Identity.Name;
                    rep.Insert(entity);
                    db.SaveChanges();
                }
                else
                {
                    entity.UpdateTime = DateTime.Now;
                    entity.UpdateUser = HttpContext.Current.User.Identity.Name;
                    rep.Update(entity);
                    db.SaveChanges();
                }

                return entity;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [Route("QuerySpecial")]
        public IEnumerable<SpecialAttendanceRule> QuerySpecial()
        {
            var rep = new SpecialAttendanceRuleRepository(db);
            return rep.Get(null, x => x.OrderBy(y => y.CreateTime));
        }
    }
}
