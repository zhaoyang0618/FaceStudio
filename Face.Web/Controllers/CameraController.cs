using Face.Contract;
using Face.Web.DAL;
using Face.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<Camera> Save(Camera entity)
        {
            if (entity == null)
                return null;

            try
            {
                var rep = new CameraRepository(db);
                if (entity.ID == Guid.Empty)
                {
                    await rep.Create(entity);
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

        [Route("ChangeLogo")]
        public async Task<IHttpActionResult> ChangeLogo(Camera camera)
        {
            //此时需要保证camera.Logo必须存在数据
            if (camera == null || camera.Logo == null)
                return BadRequest();

            if (!System.IO.File.Exists(camera.Logo.FilePath))
                return InternalServerError(new Exception("Logo图片不存在!"));

            try
            {
                //这里可能还需要保存这个Logo数据
                var rep = new CameraRepository(db);
                var ret = await rep.ChangeLogo(camera);
                if(ret)
                {
                    return Ok();
                }
                else
                {
                    return InternalServerError(new Exception("更新失败!"));
                }
            }
            catch (Exception exp)
            {
                return InternalServerError(exp);
            }
        }

        [Route("RestartDevice")]
        public async Task<IHttpActionResult> RestartDevice(Camera camera)
        {
            //此时需要保证camera.Logo必须存在数据
            if (camera == null)
                return BadRequest();

            try
            {
                var rep = new CameraRepository(db);
                var ret = await rep.RestartDevice(camera);
                if (ret)
                {
                    return Ok();
                }
                else
                {
                    return InternalServerError(new Exception("重启设备失败!"));
                }
            }
            catch (Exception exp)
            {
                return InternalServerError(exp);
            }
        }
    }
}
