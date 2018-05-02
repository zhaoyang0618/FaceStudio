using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Face.Web.Models
{
    public class MyDatabaseInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override async void Seed(ApplicationDbContext context)
        {
            //
            var user = new ApplicationUser()
            {
                UserName = "admin",
                Email = "admin@edu.cn",
            };
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            await userManager.CreateAsync(user, "123456P@d");
            base.Seed(context);
        }
    }
}