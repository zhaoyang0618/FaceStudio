using FaceStudioClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudioClient.Service
{
    public class UFaceService : BaseService
    {
        public event Action<ServiceResult<string>> OnSetPasswordCompleted;
        public event Action<ServiceResult<Person[]>> OnSetListCompleted;

        public void SetPassword(PasswordSetPara para, Action<Exception> failFunc)
        {
            var url = "http://192.168.1.123:8090/setPassWord";
            KeyValuePair<String, String>[] data = new KeyValuePair<string, string>[2];
            data[0] = new KeyValuePair<string, string>("oldPass", para.oldPass);
            data[1] = new KeyValuePair<string, string>("newPass", para.newPass);
            web.Post(data, url,
                (response, exp) =>
                    {
                        if (null != response)
                        {
                            var ret = JsonSerializer<ServiceResult<string>>.Deserialize(response);
                            if (ret != null)
                            {
                                if (null != OnSetPasswordCompleted)
                                {
                                    OnSetPasswordCompleted(ret);
                                }
                            }
                            else
                            {
                                failFunc(new Exception("Failed in Find Object!"));
                            }
                        }
                        else if (null != exp)
                        {
                            failFunc(exp);
                        }
                    });

            //var url = "http://localhost:55468/actionapi/Yolo/DoDetect";
            //web.AsyncJsonPost(url, para,
            //    (response, cookies, exp) =>
            //    {
            //        if (null != response)
            //        {
            //            var ret = JsonSerializer<ServiceResult<string>>.Deserialize(response);
            //            if (ret != null)
            //            {
            //                if (null != OnSetPasswordCompleted)
            //                {
            //                    OnSetPasswordCompleted(ret);
            //                }
            //            }
            //            else
            //            {
            //                failFunc(new Exception("Failed in Find Object!"));
            //            }
            //        }
            //        else if (null != exp)
            //        {
            //            failFunc(exp);
            //        }
            //    });
        }

        public void List(string pass, Action<Exception> failFunc)
        {
            var url = string.Format("http://192.168.1.123:8090/person/find?pass={0}&id=-1", pass);
            //KeyValuePair<String, String>[] data = new KeyValuePair<string, string>[2];
            //data[0] = new KeyValuePair<string, string>("pass", pass);
            //data[1] = new KeyValuePair<string, string>("id", "-1");
            web.Get(url,
                (response, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<ServiceResult<Person[]>>.Deserialize(response);
                        if (ret != null)
                        {
                            if (null != OnSetListCompleted)
                            {
                                OnSetListCompleted(ret);
                            }
                        }
                        else
                        {
                            failFunc(new Exception("Failed in List Person!"));
                        }
                    }
                    else if (null != exp)
                    {
                        failFunc(exp);
                    }
                });

            //var url = "http://localhost:55468/actionapi/Yolo/DoDetect";
            //web.AsyncJsonPost(url, para,
            //    (response, cookies, exp) =>
            //    {
            //        if (null != response)
            //        {
            //            var ret = JsonSerializer<ServiceResult<string>>.Deserialize(response);
            //            if (ret != null)
            //            {
            //                if (null != OnSetPasswordCompleted)
            //                {
            //                    OnSetPasswordCompleted(ret);
            //                }
            //            }
            //            else
            //            {
            //                failFunc(new Exception("Failed in Find Object!"));
            //            }
            //        }
            //        else if (null != exp)
            //        {
            //            failFunc(exp);
            //        }
            //    });
        }
    }
}
