using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace Face.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //JSON循环引用问题
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }
    }
}
/**
 * 
 * 循环引用
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml.Serialization;
 
namespace CircularReferenceSample.Models
{
    // Fix 3
    [JsonObject(IsReference = true)]
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
 
        // Fix 3
        //[JsonIgnore]
        //[IgnoreDataMember]
        public virtual ICollection<Product> Products { get; set; }
    }
 
    [DataContract(IsReference = true)]
    public class Product
    {
        [Key]
        public int Id { get; set; }
 
        [DataMember]
        public string Name { get; set; }
 
        [DataMember]
        public virtual Category Category { get; set; }
    }
}
 * **/
