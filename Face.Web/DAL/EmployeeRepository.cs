using Face.Contract;
using Face.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Face.Web.DAL
{
    public class EmployeeRepository : GenericRepository<Employee>
    {
        public EmployeeRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<PhotoImageQueryItem[]> Create(Employee entity)
        {
            entity.ID = Guid.NewGuid();
            var adapter = context as IObjectContextAdapter;
            if (entity.Deparment != null)
            {
                adapter.ObjectContext.AttachTo("Departments", entity.Deparment);
            }
            if (entity.FirstPhoto != null)
            {
                adapter.ObjectContext.AttachTo("PhotoImages", entity.FirstPhoto);
            }
            if (entity.SecondPhoto != null)
            {
                adapter.ObjectContext.AttachTo("PhotoImages", entity.SecondPhoto);
            }
            if (entity.ThirdPhoto != null)
            {
                adapter.ObjectContext.AttachTo("PhotoImages", entity.ThirdPhoto);
            }
            if (entity.AttendanceRule != null)
            {
                adapter.ObjectContext.AttachTo("AttendanceRules", entity.AttendanceRule);
            }

            //var list = new List<Camera>();
            PhotoImageQueryItem[] photoes = null;
            if (entity.Cameras != null)
            {
                foreach (var c in entity.Cameras)
                {
                    if (c.Camera != null)
                    {
                        //首先需要在设备上添加数据
                        photoes = await CreatePersonToCamera(entity, c);

                        if (c.Camera.ID != Guid.Empty)
                        {
                            //list.Add(c.Camera);
                            c.Camera = null;
                            //adapter.ObjectContext.AttachTo("Cameras", c.Camera);
                        }
                        else
                        {
                            c.Camera.ID = Guid.NewGuid();
                        }
                    }
                }
            }

            entity.CreateUser = HttpContext.Current.User.Identity.Name;
            entity.UpdateUser = HttpContext.Current.User.Identity.Name;
            Insert(entity);
            context.SaveChanges();
            return photoes;
            //if (entity.Cameras != null)
            //{
            //    foreach (var c in entity.Cameras)
            //    {
            //        foreach(var l in list)
            //        {
            //            if(c.CameraID == l.ID)
            //            {
            //                c.Camera = l;
            //                break;
            //            }
            //        }
            //    }
            //}
        }

        Service.UFaceService service = new Service.UFaceService();
        async Task<PhotoImageQueryItem[]> CreatePersonToCamera(Employee entity, EmployeeCameraRelation c)
        {
            List<PhotoImageQueryItem> list = new List<PhotoImageQueryItem>();
            var ret = await service.personCreate(c.Camera, new Person() { id = entity.Code, idcardnum = entity.IDCard, name = entity.Name });
            if (ret.success)
            {
                try
                {
                    c.PersonID = ret.data == null ? null : ret.data.id;
                    c.FirstPhotoID = null;
                    c.SecondPhotoID = null;
                    c.ThirdPhotoID = null;
                    if (entity.FirstPhoto != null)
                    {
                        var fi = new faceinfo() {
                            faceid = entity.FirstPhoto.ID.ToString().Replace("-",""),
                            personid = c.PersonID,
                            imagebase64 = Utils.Base64Converter.File2String(entity.FirstPhoto.FilePath) };
                        var photo = await service.FaceCreate(c.Camera, fi);
                        if (photo.success)
                        {
                            c.FirstPhotoID = fi.faceid;
                            var item = new PhotoImageQueryItem()
                            {
                                Camera = c.Camera,
                                PersonID = c.PersonID,
                                FaceID = c.FirstPhotoID,
                                PhotoImageID = entity.FirstPhoto.ID,
                            };
                            list.Add(item);
                        }
                        else
                        {
                            throw new Exception(string.Format( "上传第一张照片到设备({0})失败", c.Camera.IP));
                        }
                    }
                    if(entity.SecondPhoto != null)
                    {
                        var fi = new faceinfo() {
                            faceid = entity.SecondPhoto.ID.ToString().Replace("-",""),
                            personid = c.PersonID,
                            imagebase64 = Utils.Base64Converter.File2String(entity.SecondPhoto.FilePath) };
                        var photo = await service.FaceCreate(c.Camera, fi);
                        if (photo.success)
                        {
                            c.SecondPhotoID = fi.faceid;
                            var item = new PhotoImageQueryItem()
                            {
                                Camera = c.Camera,
                                PersonID = c.PersonID,
                                FaceID = c.SecondPhotoID,
                                PhotoImageID = entity.SecondPhoto.ID,
                            };
                            list.Add(item);
                        }
                        else
                        {
                            throw new Exception(string.Format("上传第二张照片到设备({0})失败", c.Camera.IP));
                        }
                    }
                    if(entity.ThirdPhoto != null)
                    {
                        var fi = new faceinfo() {
                            faceid = entity.ThirdPhoto.ID.ToString().Replace("-",""),
                            personid = c.PersonID,
                            imagebase64 = Utils.Base64Converter.File2String(entity.ThirdPhoto.FilePath) };
                        var photo = await service.FaceCreate(c.Camera, fi);
                        if (photo.success)
                        {
                            c.ThirdPhotoID = fi.faceid;
                            var item = new PhotoImageQueryItem()
                            {
                                Camera = c.Camera,
                                PersonID = c.PersonID,
                                FaceID = c.ThirdPhotoID,
                                PhotoImageID = entity.ThirdPhoto.ID,
                            };
                            list.Add(item);
                        }
                        else
                        {
                            throw new Exception(string.Format("上传第三张照片到设备({0})失败", c.Camera.IP));
                        }
                    }

                    c.Status = 1;
                }
                catch (Exception exp)
                {
                    System.Diagnostics.Debug.WriteLine(exp);
                    throw exp;
                }
                //

            }

            return list.ToArray();
        }
    }
}
