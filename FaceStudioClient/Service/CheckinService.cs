using Face.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudioClient.Service
{
    class CheckinService : BaseService
    {
        public event Action<ServiceBaseResult> OnRecordCompleted;

        public void Record(Action<Exception> failFunc)
        {
            var url = "http://localhost:8888/api/Checkin/Record";
            var para = new
            {
                ip = "192.168.1.123",
                deviceKey = "",
                personId = "",
                time = "",
                type = "",
                path = "",

            };
            web.AsyncJsonPost(url, para,
                (response, cookies, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<ServiceBaseResult>.Deserialize(response);
                        if (ret != null)
                        {
                            if (null != OnRecordCompleted)
                            {
                                OnRecordCompleted(ret);
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
    }
}
