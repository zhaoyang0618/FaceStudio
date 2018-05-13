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
    /// 在这里向各个设备轮询考勤数据；
    /// 
    /// </summary>
    public class CheckedInRecordQuery
    {
        static CheckedInRecordQuery obj = new CheckedInRecordQuery();
        public static CheckedInRecordQuery Instance
        {
            get
            {
                return obj;
            }
        }

        private CheckedInRecordQuery()
        {
        }

        object exitLock = new object();
        bool bExit = false;
        long deltaTime = 60000;

        public void Exit()
        {
            lock (exitLock)
            {
                bExit = true;
            }
        }

        bool HasExit
        {
            get
            {
                var b = false;
                lock (exitLock)
                {
                    b = bExit;
                }
                return b;
            }
        }

        //这里保证IP地址的唯一性:一台设备一个IP
        //同时存储一个时间段,没有必要重复获取数据;最好是获取到的数据直接删除
        Camera[] dictCamera = null;
        Dictionary<string, DateTime> dictDateTime = new Dictionary<string, DateTime>();

        public void RunQueryThread()
        {
            //开启一个线程,不停的获取数据，然后查询数据
            System.Threading.Thread thread =
                new System.Threading.Thread(new System.Threading.ThreadStart(async () => {
                    var db = new ApplicationDbContext();
                    var rep = new CheckinRecordRepository(db);
                    var repCamera = new CameraRepository(db);
                    var repEmployee = new EmployeeRepository(db);
                    var service = new UFaceService();
                    DateTime prev = DateTime.Now;
                    while (true)
                    {
                        if (HasExit)
                            break;

                        try
                        {
                            if (dictCamera == null || dictCamera.Length == 0)
                            {
                                //查询数据库获取相机信息
                                var cameras = repCamera.Get();
                                dictCamera = cameras.ToArray();
                            }

                            if (dictCamera == null)
                            {
                                System.Threading.Thread.Sleep(10);
                                continue;
                            }

                            foreach (var v in dictCamera)
                            {
                                if (HasExit)
                                    break;

                                Camera camera = v;
                                int length = -1;
                                int index = 0;
                                string startTime = "0";
                                string endTime = "0";
                                var now = DateTime.Now;
                                if (dictDateTime.ContainsKey(v.IP))
                                {
                                    //如果在deltaTime毫秒之内,不做处理
                                    var delta = now - dictDateTime[v.IP];
                                    if (delta.Minutes * 60000 + delta.Seconds * 1000 + delta.Milliseconds < deltaTime)
                                    {
                                        continue;
                                    }

                                    //
                                    startTime = dictDateTime[v.IP].ToString(@"yyyy-MM-dd hh\:mm\:ss");
                                    endTime = now.ToString(@"yyyy-MM-dd hh\:mm\:ss");
                                    dictDateTime[v.IP] = now;
                                }
                                else
                                {
                                    dictDateTime.Add(v.IP, now);
                                }
                                var ret = await service.findRecords(camera, "-1", length, index, startTime, endTime);
                                if (null != ret)
                                {
                                    //把数据插入数据库
                                    if (null != ret.records)
                                    {
                                        //这里面的操作都比较费时
                                        if (HasExit)
                                            break;

                                        var lst = new List<CheckinRecord>();
                                        foreach (var rec in ret.records)
                                        {
                                            //需要从数据库获取数据
                                            var peoples = repEmployee.Get(x => x.Code != null && x.Code.CompareTo(rec.personID) == 0);
                                            Employee employee = peoples == null || peoples.Count() == 0 ? null : peoples.ElementAt(0);
                                            var cr = new CheckinRecord()
                                            {
                                                InnerID = rec.ID,
                                                IP = camera.IP,
                                                Path = rec.path,
                                                PersonId = rec.personID,
                                                Time = rec.time,
                                                Type = rec.type,
                                                State = rec.state,
                                                CheckinTime = Utils.TimeHelper.UnixTime2DateTime(rec.time), //需要从unix时间戳转化得到
                                            };

                                            if (employee != null)
                                                cr.EmployeeID = employee.ID;
                                            //
                                            lst.Add(cr);
                                        }
                                        rep.Add(lst.ToArray());
                                    }
                                }
                                //
                                System.Threading.Thread.Sleep(10);
                            }
                        }
                        catch (Exception exp)
                        {
                            System.Diagnostics.Debug.WriteLine(exp);
                        }

                        System.Threading.Thread.Sleep(10);
                    }
                }));
            thread.IsBackground = true;
            thread.Start();
        }
    }
}