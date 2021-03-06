﻿using Face.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudioClient.Service
{
    class CameraService : BaseService
    {
        public event Action<Camera[]> OnQueryCompleted;
        public event Action<Camera> OnSaveCompleted;
        public event Action OnChangeLogoCompleted;
        public event Action OnRestartDeviceCompleted;

        public void Query(Action<Exception> failFunc)
        {
            var url = "http://localhost:8888/api/Camera/Query";
            web.AsyncJsonPost<string>(url, null,
                (response, cookies, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<Camera[]>.Deserialize(response);
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

        public void Save(Camera entity, Action<Exception> failFunc)
        {
            var url = "http://localhost:8888/api/Camera/Save";
            web.AsyncJsonPost(url, entity,
                (response, cookies, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<Camera>.Deserialize(response);
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

        public void ChangeLogo(Camera entity, Action<Exception> failFunc)
        {
            var url = "http://localhost:8888/api/Camera/ChangeLogo";
            web.AsyncJsonPost(url, entity,
                (response, cookies, exp) =>
                {
                    if (null != response)
                    {
                        if (OnChangeLogoCompleted != null)
                            OnChangeLogoCompleted();
                    }
                    else if (null != exp)
                    {
                        failFunc(exp);
                    }
                    else
                    {
                        if (OnChangeLogoCompleted != null)
                            OnChangeLogoCompleted();
                    }
                });
        }

        public void RestartDevice(Camera entity, Action<Exception> failFunc)
        {
            var url = "http://localhost:8888/api/Camera/RestartDevice";
            web.AsyncJsonPost(url, entity,
                (response, cookies, exp) =>
                {
                    if (null != response)
                    {
                        if (OnRestartDeviceCompleted != null)
                            OnRestartDeviceCompleted();
                    }
                    else if (null != exp)
                    {
                        failFunc(exp);
                    }
                    else
                    {
                        if (OnRestartDeviceCompleted != null)
                            OnRestartDeviceCompleted();
                    }
                });
        }
    }
}
