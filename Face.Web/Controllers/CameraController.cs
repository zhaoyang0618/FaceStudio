using Face.Contract;
using Face.Web.DAL;
using Face.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;

namespace Face.Web.Controllers
{
    [RoutePrefix("api/Camera")]
    public class CameraController : ApiController
    {
        // GET: Camera
        //public ActionResult Index()
        //{
        //    return View();
        //}
        internal ApplicationDbContext db = new ApplicationDbContext();
        // POST api/Department/Create
        [Route("Save")]
        public Camera Save(Camera entity)
        {
            if (entity == null)
                return null;

            try
            {
                var rep = new CameraRepository(db);
                if (entity.ID == Guid.Empty)
                {
                    rep.Create(entity);
                    //entity.ID = Guid.NewGuid();
                    //entity.CreateUser = HttpContext.Current.User.Identity.Name;
                    //entity.UpdateUser = HttpContext.Current.User.Identity.Name;
                    //rep.Insert(entity);
                    //db.SaveChanges();
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
        public IEnumerable<Camera> Query()
        {
            var rep = new CameraRepository(db);
            return rep.Get();
        }

        [Route("Delete")]
        public Camera Delete(Camera entiry)
        {
            var rep = new CameraRepository(db);
            rep.Delete(entiry);
            db.SaveChanges();
            return entiry;
        }
    }
}