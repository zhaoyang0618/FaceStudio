using Face.Contract;
using Face.Web.DAL;
using Face.Web.Logic;
using Face.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;

namespace Face.Web.Controllers
{
    [RoutePrefix("api/Employee")]
    public class EmployeeController : ApiController
    {
        // GET: Employee
        //public ActionResult Index()
        //{
        //    return View();
        //}
        internal ApplicationDbContext db = new ApplicationDbContext();

        [Route("Save")]
        public async Task<Employee> Save(Employee entity)
        {
            if (entity == null)
                return null;

            try
            {
                var rep = new EmployeeRepository(db);
                if (entity.ID == Guid.Empty)
                {
                    //var adapter = db as IObjectContextAdapter;
                    //if (entity.Deparment != null)
                    //{
                    //    adapter.ObjectContext.AttachTo("Departments", entity.Deparment);
                    //}
                    //if (entity.FirstPhoto != null)
                    //{
                    //    adapter.ObjectContext.AttachTo("PhotoImages", entity.FirstPhoto);
                    //}
                    //if (entity.SecondPhoto != null)
                    //{
                    //    adapter.ObjectContext.AttachTo("PhotoImages", entity.SecondPhoto);
                    //}
                    //if (entity.ThirdPhoto != null)
                    //{
                    //    adapter.ObjectContext.AttachTo("PhotoImages", entity.ThirdPhoto);
                    //}
                    //if (entity.AttendanceRule != null)
                    //{
                    //    adapter.ObjectContext.AttachTo("AttendanceRules", entity.AttendanceRule);
                    //}

                    //if (entity.Cameras != null)
                    //{
                    //    foreach (var c in entity.Cameras)
                    //    {
                    //        if (c.Camera != null)
                    //        {
                    //            if (c.Camera.ID != Guid.Empty)
                    //            {
                    //                adapter.ObjectContext.AttachTo("Cameras", c.Camera);
                    //            }
                    //            else
                    //            {
                    //                c.Camera.ID = Guid.NewGuid();
                    //            }
                    //        }
                    //    }
                    //}
                    //entity.ID = Guid.NewGuid();
                    //entity.CreateUser = HttpContext.Current.User.Identity.Name;
                    //entity.UpdateUser = HttpContext.Current.User.Identity.Name;
                    //rep.Insert(entity);
                    //db.SaveChanges();
                    var list = await rep.Create(entity);
                    PhotoFeatureQuery.Instance.AddEmployee(list);
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
        public IEnumerable<Employee> Query()
        {
            var rep = new EmployeeRepository(db);
            return rep.Get(null, x=>x.OrderBy(y=>y.CreateTime));
        }

        [Route("Delete")]
        public Employee Delete(Employee entiry)
        {
            var rep = new EmployeeRepository(db);
            rep.Delete(entiry);
            db.SaveChanges();
            return entiry;
        }

    }
}