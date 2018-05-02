using Face.Contract;
using Face.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Face.Web.DAL
{
    public class CameraRepository : GenericRepository<Camera>
    {
        public CameraRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async void Create(Camera entity)
        {
            entity.ID = Guid.NewGuid();
            //
            var ret = await service.SetPassword(entity);
            if (!ret)
            {
                throw new Exception("设置密码失败！");
            }

            var setup = new DevicePara()
            {

            };

            ret = await service.SetConfig(entity);
            if (!ret)
            {
                throw new Exception("配置参数失败！");
            }

            //
            entity.CreateTime = DateTime.Now;
            entity.CreateUser = HttpContext.Current.User.Identity.Name;
            entity.UpdateTime = DateTime.Now;
            entity.UpdateUser = HttpContext.Current.User.Identity.Name;
            Insert(entity);
            context.SaveChanges();
        }

        Service.UFaceService service = new Service.UFaceService();
    }
}
