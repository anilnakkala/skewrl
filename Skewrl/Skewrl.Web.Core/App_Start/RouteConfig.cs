using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Skewrl.Web.Core
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Default",
               url: "",
               defaults: new { controller = "Redirect", action = "GoHome" }
           );

            routes.MapRoute(
                name: "Default1",
                url: "{id}",
                defaults: new { controller = "Redirect", action = "Index" }
            );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{id}",
            //    defaults: new { controller = "Redirect", action = "Index" }
            //);
        }
    }
}