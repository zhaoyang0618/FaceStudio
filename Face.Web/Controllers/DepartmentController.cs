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
    [RoutePrefix("api/Department")]
    public class DepartmentController : ApiController
    {
        // GET: Department
        //public ActionResult Index()
        //{
        //    return View();
        //}

        internal ApplicationDbContext db = new ApplicationDbContext();
        // POST api/Department/Create
        [Route("Save")]
        public Department Save(Department entity)
        {
            if(entity == null)
                return null;

            try
            {
                var adapter = db as IObjectContextAdapter;
                if (entity.ParentDepartment != null)
                {
                    entity.ParentDepartmentID = entity.ParentDepartment.ID;
                    adapter.ObjectContext.AttachTo("Departments", entity.ParentDepartment);
                    //entity.ParentDepartment = null;
                }
                var rep = new DepartmentRepository(db);
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
        public IEnumerable<Department> Query()
        {
            var rep = new DepartmentRepository(db);
            return rep.Get(null, x=>x.OrderBy(y=>y.CreateTime));
        }

        [Route("Delete")]
        public Department Delete(Department entiry)
        {
            var rep = new DepartmentRepository(db);
            rep.Delete(entiry);
            db.SaveChanges();
            return entiry;
        }
    }
}