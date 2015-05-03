using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ChatApp.Presentation.WebMvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "ChatRoute",
            //    url: "chat/{username}",
            //    defaults: new { controller = "Home", action = "Chat", username = "" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{action}",
                defaults: new { controller = "Home" }
            );
        }
    }
}
