﻿using Face.Web.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Face.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //启动一些子线程获取数据
            PhotoFeatureQuery.Instance.RunQueryThread();
            CheckedInRecordQuery.Instance.RunQueryThread();
        }

        protected void Application_End()
        {
            PhotoFeatureQuery.Instance.Exit();
            CheckedInRecordQuery.Instance.Exit();
        }
    }
}
