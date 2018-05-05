using Face.Contract;
using Face.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.Web.Service
{
    public class UFaceService : BaseService
    {
        /****************************
         *  人员管理接口 3.1
         ***************************/
        public event Action<ServiceResult<Person[]>> OntListPersonCompleted;
        //3.1.1 /person/create
        public event Action<ServiceResult<Person>> OnCreatePersonCompleted;
        //3.1.2
        public event Action<ServiceResult<Person>> OnPersonUpdateCompleted;
        //3.1.3
        public event Action<ServiceResult<string[]>> OnPersonDeleteCompleted;
        //3.1.4
        public event Action<ServiceResult<Person[]>> OnPersonFindCompleted;
        //3.1.5
        public event Action<ServiceResult<string>> OnSetPasstimeCompleted;
        //3.1.6
        public event Action<ServiceResult<string>> OnDeletePasstimeCompleted;
        //3.1.7
        public event Action<ServiceResult<string>> OnPermissionsCreateCompleted;
        //3.1.8
        public event Action<ServiceResult<string>> OnPermissionsDeleteCompleted;


        /****************************
         *  照片管理接口 3.2`
         ***************************/
        //3.2.1: 创建照片，返回faceId. 
        public event Action<ServiceResult<string>> OnFaceCreateCompleted;
        //3.2.2
        public event Action<ServiceResult<int>> OnFaceUpdateCompleted;
        //3.2.3
        public event Action<ServiceResult<int>> OnFaceDeleteCompleted;
        //3.2.4 自动拍照
        public event Action<ServiceResult<int>> OnFaceTakeImgCompleted;
        //3.2.5 照片查询: 获取人员人脸特征信息
        public event Action<ServiceResult<faceFindInfo[]>> OnFaceFindCompleted;
        //3.2.7
        public event Action<ServiceResult<faceFindInfo>> OnFaceFeatureRegCompleted;
        //3.2.8
        public event Action<ServiceResult<int>> OnPhotoComparisionCompleted;
        //3.2.9
        public event Action<ServiceResult<int>> OnFaceDeletePersonCompleted;

        
        /****************************
         *  设备管理接口 3.3
         ***************************/
        // 3.3.1 setConfig 设备属性配置 
        public event Action<ServiceResult<DeviceConfig>> OnSetConfigCompleted;
        // 3.3.2 changeLogo 参数类型需要修改或者不需要输入参数
        public event Action<ServiceResult<string>> OnChangeLogoCompleted;
        // 3.3.3. getDeviceKey 获取序列号
        public event Action<ServiceResult<string>> OnGetDeviceKeyCompleted;

        internal void personDelete(string v1, string v2, Guid iD, Action<Exception> p)
        {
            throw new NotImplementedException();
        }

        // 3.3.4  setPassWord 设置修改密码
        public event Action<ServiceResult<string>> OnSetPasswordCompleted;
        public event Action<ServiceResult<string>> OnSetIdentifyCallBackCompleted;        
        //public event Action<ServiceResult<string>> OnSetScreenOrtCompleted;
        // 3.3.7
        public event Action<ServiceResult<NetInfo>> OnSetNetInfoCompleted;
        // 3.3.9
        public event Action<ServiceResult<string>> OnRestartDeviceCompleted;
        //setImgRegCallBack
        //setDeviceHeartBeat
        //3.3.12 setTime
        public event Action<ServiceResult<string>> OnSetTimeCompleted;
        //3.3.13 device/reset
        public event Action<ServiceResult<string>> OnResetCompleted;
        //3.3.14 device/openDoorControl
        //public event Action<ServiceResult<int>> OnOpenDoorControlCompleted;
        //device/setScreenMode
        //device/setActScreenImg


        /******************************
         * 识别记录管理接口 3.4 
         *****************************/
        //3.4.1 findRecords
        public event Action<ServiceResult<RecordsResult>> OnFindRecordsCompleted;
        //3.4.2 deleteRecords
        public event Action<ServiceResult<int>> OnDeleteRecordsCompleted;





        /**********************************************
         * 
         *  implementation 照片管理接口 3.2
         *  
         **********************************************/
        public async Task<ServiceResult<string>> FaceCreate(Camera camera, faceinfo face)
        {
            System.Diagnostics.Debug.Assert(camera != null);
            var url = string.Format("http://{0}:{1}/face/create", camera.IP, camera.Port);
            KeyValuePair<String, String>[] data = new KeyValuePair<string, string>[4];
            data[0] = new KeyValuePair<string, string>("pass", camera.Pwd);   
            data[1] = new KeyValuePair<string, string>("personId", face.personid);
            data[2] = new KeyValuePair<string, string>("faceId", face.faceid);
            data[3] = new KeyValuePair<string, string>("imgBase64", face.imagebase64);
            var response = await web.Post(data, url);
            if (!string.IsNullOrEmpty(response))
            {
                var ret = JsonSerializer<ServiceResult<string>>.Deserialize(response);
                if (ret != null)
                {
                    return ret;
                }
            }

            return null;
        }

        //3.2.2
        //public event Action<ServiceResult<DevicePara>> OnFaceUpdateCompleted;
        public void FaceUpdate(string ipAddress, string passport, faceinfo face, Action<Exception> failFunc)
        {
            var url = "http://" + ipAddress + ":8090/face/update";
            KeyValuePair<String, String>[] data = new KeyValuePair<string, string>[4];
            data[0] = new KeyValuePair<string, string>("pass", passport);
            data[1] = new KeyValuePair<string, string>("personId", face.personid);
            data[2] = new KeyValuePair<string, string>("faceId", face.faceid);
            data[3] = new KeyValuePair<string, string>("imgBase64", face.imagebase64);
            web.Post(data, url,
                (response, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<ServiceResult<int>>.Deserialize(response);
                        if (ret != null)
                        {
                            if (null != OnFaceUpdateCompleted)
                            {
                                OnFaceUpdateCompleted(ret);
                            }
                        }
                        else
                        {
                            failFunc(new Exception("Failed in face update!"));
                        }
                    }
                    else if (null != exp)
                    {
                        failFunc(exp);
                    }
                });

        }
        //3.2.3
        //public event Action<ServiceResult<int>> OnFaceDeleteCompleted;
        public void FaceDelete(string ipAddress, string passport, string faceid, Action<Exception> failFunc)
        {
            var url = "http://" + ipAddress + ":8090/face/delete";
            KeyValuePair<String, String>[] data = new KeyValuePair<string, string>[2];
            data[0] = new KeyValuePair<string, string>("pass", passport);
            data[1] = new KeyValuePair<string, string>("faceId",faceid);
            web.Post(data, url,
                (response, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<ServiceResult<int>>.Deserialize(response);
                        if (ret != null)
                        {
                            if (null != OnFaceDeleteCompleted)
                            {
                                OnFaceDeleteCompleted(ret);
                            }
                        }
                        else
                        {
                            failFunc(new Exception("Failed in face delete!"));
                        }
                    }
                    else if (null != exp)
                    {
                        failFunc(exp);
                    }
                });

        }
        //3.2.4
        //public event Action<ServiceResult<DevicePara>> OnFaceTakeImgCompleted;
        //3.2.5
        //public event Action<ServiceResult<faceFindInfo>> OnFaceFindCompleted;
        public void FaceFind(string ipAddress, string passport, string personid, Action<Exception> failFunc)
        {
            var url = "http://" + ipAddress + ":8090/face/find";
            KeyValuePair<String, String>[] data = new KeyValuePair<string, string>[2];
            data[0] = new KeyValuePair<string, string>("pass", passport);
            data[1] = new KeyValuePair<string, string>("personId", personid);
            web.Post(data, url,
                (response, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<ServiceResult<faceFindInfo[]>>.Deserialize(response);
                        if (ret != null)
                        {
                            if (null != OnFaceFindCompleted)
                            {
                                OnFaceFindCompleted(ret);
                            }
                        }
                        else
                        {
                            failFunc(new Exception("Failed in face delete!"));
                        }
                    }
                    else if (null != exp)
                    {
                        failFunc(exp);
                    }
                });

        }

        public async Task<faceFindInfo[]> FaceFind(Camera camera, string personid)
        {
            var url = string.Format( "http://{0}:{1}/face/find", camera.IP, camera.Port);
            KeyValuePair<String, String>[] data = new KeyValuePair<string, string>[2];
            data[0] = new KeyValuePair<string, string>("pass", camera.Pwd);
            data[1] = new KeyValuePair<string, string>("personId", personid);
            var response = await web.Post(data, url);
            if (null != response)
            {
                var ret = JsonSerializer<ServiceResult<faceFindInfo[]>>.Deserialize(response);
                if (ret != null && ret.success)
                {
                    return ret.data;
                }
            }

            return null;
        }

        //3.2.7
        //public event Action<ServiceResult<DevicePara>> OnFaceFeatureRegCompleted;
        public void FaceFind(string ipAddress, string passport, string personid, string faceid, string feature ,string featurekey, Action<Exception> failFunc)
        {
            var url = "http://" + ipAddress + ":8090/face/featureReg";
            KeyValuePair<String, String>[] data = new KeyValuePair<string, string>[5];
            data[0] = new KeyValuePair<string, string>("pass", passport);
            data[1] = new KeyValuePair<string, string>("personId", personid);
            data[2] = new KeyValuePair<string, string>("faceId", faceid);
            data[3] = new KeyValuePair<string, string>("feature", feature);
            data[4] = new KeyValuePair<string, string>("featureKey", featurekey);
            web.Post(data, url,
                (response, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<ServiceResult<faceFindInfo>>.Deserialize(response);
                        if (ret != null)
                        {
                            if (null != OnFaceFeatureRegCompleted)
                            {
                                OnFaceFeatureRegCompleted(ret);
                            }
                        }
                        else
                        {
                            failFunc(new Exception("Failed in face delete!"));
                        }
                    }
                    else if (null != exp)
                    {
                        failFunc(exp);
                    }
                });

        }
        //3.2.8
        //public event Action<ServiceResult<DevicePara>> OnPhotoComparisionCompleted;
        //3.2.9
        // public event Action<ServiceResult<DevicePara>> OnFaceDeletePersonCompleted;


        /**************************************
         * 
         * implementation 设备管理接口 3.3
         * 
         *************************************/
        // 3.3.1
        public void SetConfig(string passport, string ipAddress, DeviceConfig para, Action<Exception> failFunc)
        {
            var url = "http://" + ipAddress + ":8090/setConfig";
            //var json_para = new { para_pass = pass, config = para };
            //web.AsyncJsonPost(url, json_para, (response, cookie, exp) =>
            KeyValuePair<string, string>[] data = new KeyValuePair<string, string>[2];
            data[0] = new KeyValuePair<string, string>("pass", passport);
            data[1] = new KeyValuePair<string, string>("config", JsonSerializer<DeviceConfig>.Serialize(para));
            web.Post(data, url, (response, exp) => 
            {
                if (null != response)
                {
                    var ret = JsonSerializer<ServiceResult<DeviceConfig>>.Deserialize(response);
                    if (ret != null)
                    {
                        if (null != OnSetConfigCompleted)
                        {
                            OnSetConfigCompleted(ret);
                        }
                    }
                    else
                    {
                        failFunc(new Exception("Failed in set config!"));
                    }
                }
                else if (null != exp)
                {
                    failFunc(exp);
                }
            });

        }

        public async Task<bool> SetConfig(Camera c)
        {
            System.Diagnostics.Debug.Assert(c != null);
            var url = string.Format("http://{0}:{1}/setConfig", c.IP, c.Port);

            var para = new DeviceConfig()
            {
                companyName = c.Company,
                displayModContent = c.DisplayContent,
                displayModType = c.DisplayMode,
                identifyDistance = c.Identifydistance,
                identifyScores = c.Identifyscore,
                intro = c.Memo,
                multiplayerDetection = c.MultiFaceDetect,
                saveIdentifyTime = c.SaveIdentifyTime,
                //screendirection = c.ScreenDirection,
                comModContent = c.SerialPortContent,
                comModType = c.SerialPortMode,
                slogan = c.Slogan,
                recStrangerType = c.StrangerMode,
                recStrangerTimesThreshold = c.StrangerTimeThreshold,
                ttsModContent = c.TTSContent,
                ttsModType = c.TTSMode,
                ttsModStrangerContent = c.TTSStrangerContent,
                ttsModStrangerType = c.TTSStrangerMode,
            };
            KeyValuePair<string, string>[] data = new KeyValuePair<string, string>[2];
            data[0] = new KeyValuePair<string, string>("pass", c.Pwd);
            data[1] = new KeyValuePair<string, string>("config", JsonSerializer<DeviceConfig>.Serialize(para));
            var response = await web.Post(data, url);
            if (!string.IsNullOrEmpty(response))
            {
                var ret = JsonSerializer<ServiceResult<DeviceConfig>>.Deserialize(response);
                if (ret != null)
                {
                    return ret.success;
                }
            }

            return false;
        }

        //3.3.2  图片怎么传参数？？？？
        public void ChangeLogo(string pass, string ipAddress, string imgBase64, Action<Exception> failFunc)
        {
            var url = string.Format("http://{0}:8090/changeLogo?pass={1}&imgBase64={2}", ipAddress, pass, imgBase64);
            web.Get(url,
               (response, exp) =>
               {
                   if (null != response)
                   {
                       var ret = JsonSerializer<ServiceResult<string>>.Deserialize(response);
                       if (ret != null)
                       {
                           if (null != OnChangeLogoCompleted)
                           {
                               OnChangeLogoCompleted(ret);
                           }
                       }
                       else
                       {
                           failFunc(new Exception("Failed in change logo!"));
                       }
                   }
                   else if (null != exp)
                   {
                       failFunc(exp);
                   }
               });

        }
        //3.3.3 
        public void GetDeviceKey(string pass, string ipAddress, Action<Exception> failFunc)
        {
            var url = string.Format("http://{0}:8090/changeLogo?pass={1}", ipAddress, pass);
            web.Get(url,
               (response, exp) =>
               {
                   if (null != response)
                   {
                       var ret = JsonSerializer<ServiceResult<string>>.Deserialize(response);
                       if (ret != null)
                       {
                           if (null != OnGetDeviceKeyCompleted)
                           {
                               OnGetDeviceKeyCompleted(ret);
                           }
                       }
                       else
                       {
                           failFunc(new Exception("Failed in change logo!"));
                       }
                   }
                   else if (null != exp)
                   {
                       failFunc(exp);
                   }
               });
        }

        //创建设备的时候使用
        public async Task<bool> SetPassword(Camera camera)
        {
            System.Diagnostics.Debug.Assert(null != camera);
            var url = string.Format("http://{0}:{1}/setPassWord", camera.IP, camera.Port);
            KeyValuePair<String, String>[] data = new KeyValuePair<string, string>[2];
            data[0] = new KeyValuePair<string, string>("oldPass", camera.OldPwd);
            data[1] = new KeyValuePair<string, string>("newPass", camera.Pwd);
            var response = await web.Post(data, url);
            if (!string.IsNullOrEmpty(response))
            {
                var ret = JsonSerializer<ServiceResult<string>>.Deserialize(response);
                if (ret != null)
                {
                    return ret.success;
                }
            }

            return false;
        }
        //3.3.5 192.168.1.106：59710/api/checkin/record
        public void SetIdentifyCallBack(string ipAddress, string pass, string callbackUrl, Action<Exception> failFunc)
        {
            var url = string.Format("http://{0}:8090/setIdentifyCallBack", ipAddress);
            KeyValuePair<String, String>[] data = new KeyValuePair<string, string>[2];
            data[0] = new KeyValuePair<string, string>("pass", pass);
            data[1] = new KeyValuePair<string, string>("callbackUrl", callbackUrl);
            web.Post(data, url,
                (response, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<ServiceResult<string>>.Deserialize(response);
                        if (ret != null)
                        {
                            if (null != OnSetIdentifyCallBackCompleted)
                            {
                                OnSetIdentifyCallBackCompleted(ret);
                            }
                        }
                        else
                        {
                            failFunc(new Exception("Failed in Set Password!"));
                        }
                    }
                    else if (null != exp)
                    {
                        failFunc(exp);
                    }
                });

        }
        //3.3.6 setScreenOrt
        //3.3.7.1 SetStaticIpAddress: 设置静态网络地址
        public void SetStaticIpAddress(string ipAddress, string passport, string staticIpAddress, Action<Exception> failFunc)
        {
            var url = string.Format("http://{0}:8090/setNetInfo", ipAddress);
            KeyValuePair<String, String>[] data = new KeyValuePair<string, string>[6];
            data[0] = new KeyValuePair<string, string>("pass", passport);
            data[1] = new KeyValuePair<string, string>("isDHCPMod", "2");
            data[2] = new KeyValuePair<string, string>("ip", "");
            data[3] = new KeyValuePair<string, string>("gateway", "");
            data[4] = new KeyValuePair<string, string>("subnetMask", "");
            data[5] = new KeyValuePair<string, string>("DNS", "");

            web.Post(data, url,
               (response, exp) =>
               {
                   if (null != response)
                   {
                       var ret = JsonSerializer<ServiceResult<NetInfo>>.Deserialize(response);
                       if (ret != null)
                       {
                           if (null != OnSetNetInfoCompleted)
                           {
                               OnSetNetInfoCompleted(ret);
                           }
                       }
                       else
                       {
                           failFunc(new Exception("Failed in set static ip address!"));
                       }
                   }
                   else if (null != exp)
                   {
                       failFunc(exp);
                   }
               });
        }
        //3.3.7 setNetInfo
        public void SetNetInfo(string ipAddress, string passport, int dhcpMode, string ipAddress1, string gateway, string subNetMark, string dns, Action<Exception> failFunc)
        {
            var url = string.Format("http://{0}:8090/setNetInfo", ipAddress);
            KeyValuePair<String, String>[] data = new KeyValuePair<string, string>[6];
            data[0] = new KeyValuePair<string, string>("pass", passport);
            if (dhcpMode == 2)
            {
                data[1] = new KeyValuePair<string, string>("isDHCPMod", "2");
                data[2] = new KeyValuePair<string, string>("ip", ipAddress1);
                data[3] = new KeyValuePair<string, string>("gateway", gateway);
                data[4] = new KeyValuePair<string, string>("subnetMask", subNetMark);
                data[5] = new KeyValuePair<string, string>("DNS", dns);
            }else
            {
                data[1] = new KeyValuePair<string, string>("isDHCPMod", "1");
                data[2] = new KeyValuePair<string, string>("ip", "");
                data[3] = new KeyValuePair<string, string>("gateway", "");
                data[4] = new KeyValuePair<string, string>("subnetMask", "");
                data[5] = new KeyValuePair<string, string>("DNS", "");
            }

            web.Post(data, url,
               (response, exp) =>
               {
                   if (null != response)
                 {
                     var ret = JsonSerializer<ServiceResult<NetInfo>>.Deserialize(response);
                     if (ret != null)
                     {
                         if (null != OnSetNetInfoCompleted)
                         {
                               OnSetNetInfoCompleted(ret);
                         }
                     }
                     else
                     {
                         failFunc(new Exception("Failed in set net info!"));
                     }
                 }
                 else if (null != exp)
                 {
                     failFunc(exp);
                 }
             });
        }
        //3.3.9 重启设备
        public void RestartDevice(string pass, string ipAddress, Action<Exception> failFunc)
        {
            var url = string.Format("http://{0}:8090/restartDevice?pass={1}", ipAddress, pass);
            web.Get(url,
               (response, exp) =>
               {
                   if (null != response)
                   {
                       var ret = JsonSerializer<ServiceResult<string>>.Deserialize(response);
                       if (ret != null)
                       {
                           if (null != OnRestartDeviceCompleted)
                           {
                               OnRestartDeviceCompleted(ret);
                           }
                       }
                       else
                       {
                           failFunc(new Exception("Failed in restart device!"));
                       }
                   }
                   else if (null != exp)
                   {
                       failFunc(exp);
                   }
               });
        }
        //3.3.12 setTime:Unix毫秒级时间戳
        public void Reset(string passport, string ipAddress, string time, Action<Exception> failFunc)
        {
            var url = string.Format("http://{0}:8090/device/setTime", ipAddress);
            KeyValuePair<string, string>[] data = new KeyValuePair<string, string>[2];
            data[0] = new KeyValuePair<string, string>("pass", passport);
            data[1] = new KeyValuePair<string, string>("time", time);
            web.Post(data, url,
                (response, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<ServiceResult<string>>.Deserialize(response);
                        if (ret != null)
                        {
                            if (null != OnSetTimeCompleted)
                            {
                                OnSetTimeCompleted(ret);
                            }
                        }
                        else
                        {
                            failFunc(new Exception("Failed in reset device!"));
                        }
                    }
                    else if (null != exp)
                    {
                        failFunc(exp);
                    }
                });
        }

        //3.3.13 重置设备：删除数据，删除接口配置（true，false）
        public void Reset(string passport, string ipAddress, Boolean delete, Action<Exception> failFunc)
        {
            var url = string.Format("http://{0}:8090/device/reset", ipAddress);
            KeyValuePair<string, string>[] data = new KeyValuePair<string , string>[2];
            data[0] = new KeyValuePair<string, string>("pass", passport);
            if (delete == true)
                data[1] = new KeyValuePair<string, string>("time", "true");
            else
                data[1] = new KeyValuePair<string, string>("time", "false");
            web.Post(data, url,
                (response, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<ServiceResult<string>>.Deserialize(response);
                        if (ret != null)
                        {
                            if (null != OnResetCompleted)
                            {
                                OnResetCompleted(ret);
                            }
                        }
                        else
                        {
                            failFunc(new Exception("Failed in reset device!"));
                        }
                    }
                    else if (null != exp)
                    {
                        failFunc(exp);
                    }
                });
        }

        /**************************************
         * 
         * implementation 识别记录管理接口 3.4
         * 
         *************************************/
        /* 3.4.1 findRecords
         * 时间格式：2017-07-15 12:05:00
         * lenth = -1不分页
         * index 0
         * personID = -1全部人员
         * */
        public void findRecords(string pass, string ipAddress, string personID, int length, int index, string startTime, string endTime, Action<Exception> failFunc)
        {
            var url = string.Format("http://{0}:8090/findRecords?pass={1}&personId={2}&length={3}&index={4}&startTime={5}&endTime={6}", ipAddress, pass, personID, length, index, startTime, endTime);
            web.Get(url,
               (response, exp) =>
               {
                   if (null != response)
                   {
                       var ret = JsonSerializer<ServiceResult<RecordsResult>>.Deserialize(response);
                       if (ret != null)
                       {
                           if (null != OnFindRecordsCompleted)
                           {
                               OnFindRecordsCompleted(ret);
                           }
                       }
                       else
                       {
                           failFunc(new Exception("Failed in find records!"));
                       }
                   }
                   else if (null != exp)
                   {
                       failFunc(exp);
                   }
               });
        }

        public async Task<RecordsResult> findRecords(Camera camera, string personID, int length, int index, string startTime, string endTime)
        {
            var url = string.Format("http://{0}:{1}/findRecords?pass={2}&personId={3}&length={4}&index={5}&startTime={6}&endTime={7}", camera.IP, camera.Port, camera.Pwd, personID, length, index, startTime, endTime);
            var response = await web.Get(url);
            if (!string.IsNullOrEmpty(response))
            {
                var ret = JsonSerializer<ServiceResult<RecordsResult>>.Deserialize(response);
                if (ret != null)
                {
                    return ret.data;
                }
            }

            return null;
        }

        /* 3.4.1 deleteRecords
          * 时间格式：2017-07-15 12:05:00
          * 删除指定时间前的所有记录，返回删除的数量
          * */
        public void deleteRecords(string passport, string ipAddress, string time, Action<Exception> failFunc)
        {
            var url = string.Format("http://{0}:8090/deleteRecords", ipAddress);
            KeyValuePair<String, String>[] data = new KeyValuePair<string, string>[2];
            data[0] = new KeyValuePair<string, string>("pass", passport);
            data[1] = new KeyValuePair<string, string>("time", time);
            web.Post(data, url,
                (response, exp) =>
                {
                    if (null != response)
                   {
                       var ret = JsonSerializer<ServiceResult<int>>.Deserialize(response);
                       if (ret != null)
                       {
                           if (null != OnDeleteRecordsCompleted)
                           {
                               OnDeleteRecordsCompleted(ret);
                           }
                       }
                       else
                       {
                           failFunc(new Exception("Failed in delete records!"));
                       }
                   }
                   else if (null != exp)
                   {
                       failFunc(exp);
                   }
               });
        }


        /**************************************
         * 
         * implementation 人员管理接口 3.1
         * 
         *************************************/
        //3.1.1:person的传入参数可以为空，id会自动生成
        public async Task<ServiceResult<Person>> personCreate(Camera camera, Person para)
        {
            System.Diagnostics.Debug.Assert(null != camera);

            var url = string.Format("http://{0}:{1}/person/create", camera.IP, camera.Port);
            KeyValuePair<String, String>[] data = new KeyValuePair<string, string>[2];
            data[0] = new KeyValuePair<string, string>("pass", camera.Pwd);
            data[1] = new KeyValuePair<string, string>("person", JsonSerializer<Person>.Serialize(para));
            var response = await web.Post(data, url);
            if (!string.IsNullOrEmpty(response))
            {
                var ret = JsonSerializer<ServiceResult<Person>>.Deserialize(response);
                if (ret != null)
                {
                    return ret;
                }
            }

            return null;
        }

        //3.1.2:
        public void personUpdate(string ipAddress, string passport, Person para, Action<Exception> failFunc)
        {
            var url = "http://" + ipAddress + ":8090/person/update";
            //var json_para = new { pass = passport, person = para };
            //web.AsyncJsonPost(url, json_para, (response, cookie, exp) =>
            KeyValuePair<string, string>[] data = new KeyValuePair<string, string>[2];
            data[0] = new KeyValuePair<string, string>("pass",passport);
            data[1] = new KeyValuePair<string, string>("person", JsonSerializer<Person>.Serialize(para));
            web.Post(data, url, (response,exp) =>
            {
                if (null != response)
                {
                    
                    var ret = JsonSerializer<ServiceResult<Person>>.Deserialize(response);
                    if (ret != null)
                    {
                        if (null != OnPersonUpdateCompleted)
                        {
                            OnPersonUpdateCompleted(ret);
                        }
                    }
                    else
                    {
                        failFunc(new Exception("Failed in update person!"));
                    }
                                       
                }
                else if (null != exp)
                {
                    failFunc(exp);
                }
            });
        }
        //3.1.3: id=-1表示全部删除
        public void personDelete(string ipAddress, string passport, string id, Action<Exception> failFunc)
        {
            var url = string.Format("http://{0}:8090/person/delete", ipAddress);
            KeyValuePair<String, String>[] data = new KeyValuePair<string, string>[2];
            data[0] = new KeyValuePair<string, string>("pass", passport);
            data[1] = new KeyValuePair<string, string>("id", id);
            web.Post(data, url,
                (response, exp) =>
            {
                   if (null != response)
                {
                    /*
                    var ret = JsonSerializer<ServiceResult<string[]>>.Deserialize(response);
                    if (ret != null)
                    {
                        if (null != OnPersonDeleteCompleted)
                        {
                            OnPersonDeleteCompleted(ret);
                        }
                    }
                    else
                    {
                        failFunc(new Exception("Failed in delete person!"));
                    }*/
                }
                else if (null != exp)
                {
                    failFunc(exp);
                }
            });
        }
        //3.1.4: id=-1表示全部查询
        public void personFind(string ipAddress, string passport, string id, Action<Exception> failFunc)
        {
            var url = string.Format("http://{0}:8090/person/find?pass={1}&id={2}", ipAddress, passport, id);
            web.Get(url,
               (response, exp) =>
               {
                   if (null != response)
                   {
                       var ret = JsonSerializer<ServiceResult<Person[]>>.Deserialize(response);
                       if (ret != null)
                       {
                           if (null != OnPersonFindCompleted)
                           {
                               OnPersonFindCompleted(ret);
                           }
                       }
                       else
                       {
                           failFunc(new Exception("Failed in find person!"));
                       }
                   }
                   else if (null != exp)
                   {
                       failFunc(exp);
                   }
               });
        }
        //3.1.5:设置权限时间（最多三段)
        public void createPasstime(string ipAddress, string passport, passTime para, Action<Exception> failFunc)
        {
            var url = "http://" + ipAddress + ":8090/person/createPasstime";
            KeyValuePair<string, string>[] data = new KeyValuePair<string, string>[2];
            data[0] = new KeyValuePair<string, string> ("pass",passport);
            data[1] = new KeyValuePair<string, string>("passTime", JsonSerializer<passTime>.Serialize(para));
            web.Post(data, url,
                (response, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<ServiceResult<string>>.Deserialize(response);
                        if (ret != null)
                        {
                            if (null != OnSetPasstimeCompleted)
                            {
                                OnSetPasstimeCompleted(ret);
                            }
                        }
                        else
                        {
                            failFunc(new Exception("Failed in create passtime!"));
                        }
                    }
                    else if (null != exp)
                    {
                        failFunc(exp);
                    }
                });
            /*
            var json_para = new { pass = passport, passtime = para };
            web.AsyncJsonPost(url, json_para, (response, cookie, exp) =>
            {
                if (null != response)
                {
                    var ret = JsonSerializer<ServiceResult<string>>.Deserialize(response);
                    if (ret != null)
                    {
                        if (null != OnSetPasstimeCompleted)
                        {
                            OnSetPasstimeCompleted(ret);
                        }
                    }
                    else
                    {
                        failFunc(new Exception("Failed in create passtime!"));
                    }
                }
                else if (null != exp)
                {
                    failFunc(exp);
                }
            });
            */
        }

        //3.1.6:删除权限时间
        public void deletePasstime(string ipAddress, string passport, string id, Action<Exception> failFunc)
        {
            var url = string.Format("http://{0}:8090/person/deletePasstime", ipAddress);
            KeyValuePair<String, String>[] data = new KeyValuePair<string, string>[2];
            data[0] = new KeyValuePair<string, string>("pass", passport);
            data[1] = new KeyValuePair<string, string>("personId", id);
            web.Post(data, url,
                (response, exp) =>
                {
                    if (null != response)
                {
                    var ret = JsonSerializer<ServiceResult<string>>.Deserialize(response);
                    if (ret != null)
                    {
                        if (null != OnDeletePasstimeCompleted)
                        {
                                OnDeletePasstimeCompleted(ret);
                        }
                    }
                    else
                    {
                        failFunc(new Exception("Failed in delete passtime!"));
                    }
                }
                else if (null != exp)
                {
                    failFunc(exp);
                }
            });
        }
        //3.1.7:设置权限到期日期-
        public void createPermisss(string ipAddress, string passport, string para, Action<Exception> failFunc)
        {
            var url = string.Format("http://{0}:8090/person/permissionsCreate", ipAddress);
            KeyValuePair<String, String>[] data = new KeyValuePair<string, string>[2];
            data[0] = new KeyValuePair<string, string>("pass", passport);
            data[1] = new KeyValuePair<string, string>("time", para);
            web.Post(data, url,
                (response, exp) =>
                {
                    if (null != response)
                {
                    var ret = JsonSerializer<ServiceResult<string>>.Deserialize(response);
                    if (ret != null)
                    {
                        if (null != OnPermissionsCreateCompleted)
                        {
                                OnPermissionsCreateCompleted(ret);
                        }
                    }
                    else
                    {
                        failFunc(new Exception("Failed in create permissions!"));
                    }
                }
                else if (null != exp)
                {
                    failFunc(exp);
                }
            });
        }

        //3.1.8:删除权限到期日期
        public void deletePermisss(string ipAddress, string passport, string id, Action<Exception> failFunc)
        {
            var url = string.Format("http://{0}:8090/person/permissionsDelete", ipAddress);
            KeyValuePair<String, String>[] data = new KeyValuePair<string, string>[2];
            data[0] = new KeyValuePair<string, string>("pass", passport);
            data[1] = new KeyValuePair<string, string>("personId", id);
            web.Post(data, url,
                (response, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<ServiceResult<string>>.Deserialize(response);
                        if (ret != null)
                        {
                            if (null != OnPermissionsDeleteCompleted)
                            {
                                OnPermissionsDeleteCompleted(ret);
                            }
                        }
                        else
                        {
                            failFunc(new Exception("Failed in delete permissions!"));
                        }
                    }
                    else if (null != exp)
                    {
                        failFunc(exp);
                    }
                });
        }

        public void CreatePerson(Person para, Action<Exception> failFunc)
        {
            var url = "http://192.168.1.123:8090/person/create";
            web.AsyncJsonPost(url, para,
                (response, cookie, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<ServiceResult<Person>>.Deserialize(response);
                        if (ret != null)
                        {
                            if (null != OnCreatePersonCompleted)
                            {
                                OnCreatePersonCompleted(ret);
                            }
                        }
                        else
                        {
                            failFunc(new Exception("Failed in Create Person!"));
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
                            if (null != OntListPersonCompleted)
                            {
                                OntListPersonCompleted(ret);
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
