using Face.Contract;
using Face.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Face.Web.DAL
{
    public class EmployeeRepository : GenericRepository<Employee>
    {
        public EmployeeRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public void Create(Employee entity)
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

            if (entity.Cameras != null)
            {
                foreach (var c in entity.Cameras)
                {
                    if (c.Camera != null)
                    {
                        //首先需要在设备上添加数据
                        CreatePersonToCamera(entity, c);

                        if (c.Camera.ID != Guid.Empty)
                        {
                            adapter.ObjectContext.AttachTo("Cameras", c.Camera);
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
        }

        Service.UFaceService service = new Service.UFaceService();
        async void CreatePersonToCamera(Employee entity, EmployeeCameraRelation c)
        {
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
                        var photo = await service.FaceCreate(c.Camera, new faceinfo() { faceid = entity.FirstPhoto.ID.ToString(), personid = c.PersonID, imagebase64 = Utils.Base64Converter.File2String(entity.FirstPhoto.FilePath) });
                        if (photo.success)
                        {
                            c.FirstPhotoID = photo.data;
                        }
                        else
                        {
                            throw new Exception(string.Format( "上传第一张照片到设备({0})失败", c.Camera.IP));
                        }
                    }
                    if(entity.SecondPhoto != null)
                    {
                        var photo = await service.FaceCreate(c.Camera, new faceinfo() { faceid = entity.SecondPhoto.ID.ToString(), personid = c.PersonID, imagebase64 = Utils.Base64Converter.File2String(entity.SecondPhoto.FilePath) });
                        if (photo.success)
                        {
                            c.SecondPhotoID = photo.data;
                        }
                        else
                        {
                            throw new Exception(string.Format("上传第二张照片到设备({0})失败", c.Camera.IP));
                        }
                    }
                    if(entity.ThirdPhoto != null)
                    {
                        var photo = await service.FaceCreate(c.Camera, new faceinfo() { faceid = entity.ThirdPhoto.ID.ToString(), personid = c.PersonID, imagebase64 = Utils.Base64Converter.File2String(entity.ThirdPhoto.FilePath) });
                        if (photo.success)
                        {
                            c.ThirdPhotoID = photo.data;
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
                }
                //

            }
        }
    }
}
