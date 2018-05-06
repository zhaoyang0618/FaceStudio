using Face.Contract;
using Face.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Face.Web.DAL
{
    public class CameraRepository : GenericRepository<Camera>
    {
        public CameraRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task Create(Camera entity)
        {
            entity.ID = Guid.NewGuid();
            //
            var ret = await service.SetPassword(entity);
            if (!ret)
            {
                throw new Exception("设置密码失败！");
            }

            ret = await service.SetConfig(entity);
            if (!ret)
            {
                throw new Exception("配置参数失败！");
            }

            System.Diagnostics.Debug.WriteLine("+++++++++");

            //
            entity.CreateTime = DateTime.Now;
            entity.CreateUser = HttpContext.Current.User.Identity.Name;
            entity.UpdateTime = DateTime.Now;
            entity.UpdateUser = HttpContext.Current.User.Identity.Name;
            Insert(entity);
            context.SaveChanges();
        }

        Service.UFaceService service = new Service.UFaceService();

        public async Task<bool> ChangeLogo(Camera camera)
        {
            if (camera == null || camera.Logo == null)
                return false;
            //更新设备
            var ret = await service.ChangeLogo(camera);
            //保存数据
            camera.PhotoImageID = camera.Logo.ID;
            camera.UpdateTime = DateTime.Now;
            camera.Logo = null;
            dbSet.Attach(camera);
            DbHelper<Camera>.DbColumnsUpdate(context, camera,
                new String[] { "PhotoImageID", "UpdateTime" });
            context.SaveChanges();
            return ret;
        }

        public async Task<bool> RestartDevice(Camera camera)
        {
            return await service.RestartDevice(camera);
        }
    }
}
