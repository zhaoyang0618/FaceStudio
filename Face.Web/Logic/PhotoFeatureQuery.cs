using Face.Contract;
using Face.Web.DAL;
using Face.Web.Models;
using Face.Web.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Face.Web.Logic
{
    /// <summary>
    /// 在这里完成照片的特征数据的获取；
    /// 创建照片关联人员之后进行获取
    /// </summary>
    public class PhotoFeatureQuery
    {
        static PhotoFeatureQuery obj = new PhotoFeatureQuery();
        public static PhotoFeatureQuery Instance
        {
            get
            {
                return obj;
            }
        }

        private PhotoFeatureQuery()
        {
        }

        object exitLock = new object();
        bool bExit = false;
        object queueLock = new object();
        Queue<PhotoImageQueryItem> queuePhoto = new Queue<PhotoImageQueryItem>();

        public void Exit()
        {
            lock(exitLock)
            {
                bExit = true;
            }
        }

        public void AddEmployee(PhotoImageQueryItem[] list)
        {
            if (list == null) return;
            //List<PhotoImageQueryItem> list = new List<PhotoImageQueryItem>();
            //if(employee.Cameras != null && employee.Cameras.Count > 0)
            //{
            //    var c = employee.Cameras[0];
            //    if(!string.IsNullOrEmpty(c.FirstPhotoID))
            //    {
            //        var item = new PhotoImageQueryItem()
            //        {
            //            Camera = c.Camera,
            //            PersonID = c.PersonID,
            //            FaceID = c.FirstPhotoID,
            //            PhotoImageID = employee.FirstPhoto.ID,
            //        };
            //        list.Add(item);
            //    }
            //    if (!string.IsNullOrEmpty(c.SecondPhotoID))
            //    {
            //        var item = new PhotoImageQueryItem()
            //        {
            //            Camera = c.Camera,
            //            PersonID = c.PersonID,
            //            FaceID = c.SecondPhotoID,
            //            PhotoImageID = employee.SecondPhoto.ID,
            //        };
            //        list.Add(item);
            //    }
            //    if (!string.IsNullOrEmpty(c.ThirdPhotoID))
            //    {
            //        var item = new PhotoImageQueryItem()
            //        {
            //            Camera = c.Camera,
            //            PersonID = c.PersonID,
            //            FaceID = c.ThirdPhotoID,
            //            PhotoImageID = employee.ThirdPhoto.ID,
            //        };
            //        list.Add(item);
            //    }
            //}

            if (list.Length == 0)
                return;

            lock (queueLock)
            {
                foreach(var v in list)
                {
                    queuePhoto.Enqueue(v);
                }
            }
        }

        public void RunQueryThread()
        {
            //开启一个线程,不停的获取数据，然后查询数据
            System.Threading.Thread thread =
                new System.Threading.Thread(new System.Threading.ThreadStart(async () =>{
                    var db = new ApplicationDbContext();
                    var rep = new PhotoImageRepository(db);
                    var service = new UFaceService();
                    while (true)
                    {
                        var b = false;
                        lock(exitLock)
                        {
                            b = bExit;
                        }
                        if (b)
                            break;

                        if (queuePhoto.Count == 0)
                        {
                            System.Threading.Thread.Sleep(10);
                            continue;
                        }

                        PhotoImageQueryItem item = null;
                        lock (queueLock)
                        {
                            item = queuePhoto.Dequeue();
                        }

                        if (item == null) continue;

                        try
                        {
                            //查询数据
                            var list = await service.FaceFind(item.Camera, item.PersonID);
                            if (list != null && list.Length > 0)
                            {
                                //找到对应的;然后更新
                                foreach (var v in list)
                                {
                                    if (string.Compare(v.faceId, item.FaceID) == 0)
                                    {
                                        var photo = new PhotoImage()
                                        {
                                            ID = item.PhotoImageID,
                                            Feature = v.feature,
                                            FeatureKey = v.featureKey,
                                        };
                                        rep.UpdateFeature(photo, null);
                                        break;
                                    }
                                }
                            }
                        }
                        catch (Exception exp)
                        {
                            System.Diagnostics.Debug.WriteLine(exp);
                        }
                        //
                        System.Threading.Thread.Sleep(10);
                    }
                }));
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
