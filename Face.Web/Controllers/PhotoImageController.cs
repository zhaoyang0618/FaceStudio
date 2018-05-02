using Face.Web.DAL;
using Face.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Face.Web.Controllers
{
    public class PhotoImageController : Controller
    {
        internal ApplicationDbContext db = new ApplicationDbContext();

        // GET: PhotoImage
        public ActionResult Index(Guid? id)
        {
            //return View();
            if (null == id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var rep = new PhotoImageRepository(db);
            var file = rep.GetByID(id);
            if (null == file)
                return HttpNotFound("文件不存在!");

            //首先从缓存获取相关数据
            string filepath = file.FilePath;
            if (System.IO.File.Exists(filepath))
            {
                //读取文件数据
                var data = System.IO.File.ReadAllBytes(filepath);
                return File(data, file.MimeType, file.FileName);
            }
            else
            {
                return HttpNotFound("文件不存在!");
            }
        }

        //[Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Upload(HttpPostedFileBase fileToUpload)
        {
            if (null == fileToUpload)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var rep = new PhotoImageRepository(db);
                //var file = rep.Create(fileToUpload, HttpContext.User.Identity.Name);
                var file = rep.Create(fileToUpload, "admin");
                return Json(file);
            }
            catch (Exception exp)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, exp.Message);
            }
        }
    }
}