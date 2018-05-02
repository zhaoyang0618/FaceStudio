using Face.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudioClient.Service
{
    class AttendanceRuleService : BaseService
    {
        public event Action<AttendanceRule[]> OnQueryCompleted;
        public event Action<AttendanceRule> OnSaveCompleted;
        public event Action<SpecialAttendanceRule[]> OnQuerySpecialCompleted;
        public event Action<SpecialAttendanceRule> OnSaveSpecialCompleted;

        public void Query(Action<Exception> failFunc)
        {
            var url = "http://localhost:59710/api/AttendanceRule/Query";
            web.AsyncJsonPost<string>(url, null,
                (response, cookies, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<AttendanceRule[]>.Deserialize(response);
                        if (ret != null)
                        {
                            if (null != OnQueryCompleted)
                            {
                                OnQueryCompleted(ret);
                            }
                        }
                        else
                        {
                            failFunc(new Exception("Failed in Query Object!"));
                        }
                    }
                    else if (null != exp)
                    {
                        failFunc(exp);
                    }
                });
        }

        public void Save(AttendanceRule entity, Action<Exception> failFunc)
        {
            var url = "http://localhost:59710/api/AttendanceRule/Save";
            web.AsyncJsonPost(url, entity,
                (response, cookies, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<AttendanceRule>.Deserialize(response);
                        if (ret != null)
                        {
                            if (null != OnSaveCompleted)
                            {
                                OnSaveCompleted(ret);
                            }
                        }
                        else
                        {
                            failFunc(new Exception("Failed in Save Object!"));
                        }
                    }
                    else if (null != exp)
                    {
                        failFunc(exp);
                    }
                });
        }

        public void QuerySpecial(Action<Exception> failFunc)
        {
            var url = "http://localhost:59710/api/AttendanceRule/QuerySpecial";
            web.AsyncJsonPost<string>(url, null,
                (response, cookies, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<SpecialAttendanceRule[]>.Deserialize(response);
                        if (ret != null)
                        {
                            if (null != OnQuerySpecialCompleted)
                            {
                                OnQuerySpecialCompleted(ret);
                            }
                        }
                        else
                        {
                            failFunc(new Exception("Failed in Query Object!"));
                        }
                    }
                    else if (null != exp)
                    {
                        failFunc(exp);
                    }
                });
        }

        public void SaveSpecial(SpecialAttendanceRule entity, Action<Exception> failFunc)
        {
            var url = "http://localhost:59710/api/AttendanceRule/SaveSpecial";
            web.AsyncJsonPost(url, entity,
                (response, cookies, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<SpecialAttendanceRule>.Deserialize(response);
                        if (ret != null)
                        {
                            if (null != OnSaveSpecialCompleted)
                            {
                                OnSaveSpecialCompleted(ret);
                            }
                        }
                        else
                        {
                            failFunc(new Exception("Failed in Save Object!"));
                        }
                    }
                    else if (null != exp)
                    {
                        failFunc(exp);
                    }
                });
        }
    }
}
