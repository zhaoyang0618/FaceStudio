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
    /// <summary>
    /// 考勤记录:其中Callback用于和考勤机器的回调
    /// </summary>
    [RoutePrefix("api/Checkin")]
    public class CheckinController : ApiController
    {
        // GET: Checkin
        //public ActionResult Index()
        //{
        //    return View();
        //}
        internal ApplicationDbContext db = new ApplicationDbContext();

        [Route("Record")]
        public ServiceBaseResult Record(CheckinRecord rec)
        {
            var ret = new ServiceBaseResult();
            try
            {
                ret.result = 1;
                ret.success = true;
            }
            catch(Exception exp)
            {
                ret.result = 1;
                ret.success = false;
            }
            return ret;
        }

        public IEnumerable<CheckinRecord> Query(CheckinRecordQueryInfo q)
        {
            var rep = new CheckinRecordRepository(db);
            return rep.Get();
        }
    }
}
