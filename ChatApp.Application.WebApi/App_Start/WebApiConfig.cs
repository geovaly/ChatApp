using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ChatApp.Application.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();


            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "api/{controller}/{action}");
        }
    }
}
