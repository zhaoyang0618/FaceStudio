using Face.Contract;
using Face.Web.DAL;
using Face.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
//using System.Net.Http;
using System.Web.Http;

namespace Face.Web.Controllers
{
    [RoutePrefix("api/File")]
    public class FileController : ApiController
    {
        internal ApplicationDbContext db = new ApplicationDbContext();
        public FileController()
        {
        }

        [Route("Upload")]
        public PhotoImage Upload(FileData entity)
        {
            if (null == entity)
            {
                return null;
            }

            try
            {
                var rep = new PhotoImageRepository(db);
                var file = rep.Create(entity.Data, entity.FileName, "admin");
                return file;
            }
            catch (Exception exp)
            {
                return null;
            }
        }
    }
}
