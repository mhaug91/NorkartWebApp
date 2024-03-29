﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Api
{
    public static class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            /* // Controller Only
             // To handle routes like `/api/VTRouting`
             config.Routes.MapHttpRoute(
                 name: "ControllerOnly",
                 routeTemplate: "api/{controller}",
                 defaults: new { id = RouteParameter.Optional }
             );
             // Controllers with Actions
             // To handle routes like `/api/VTRouting/route`
             config.Routes.MapHttpRoute(
                 name: "ControllerAndAction",
                 routeTemplate: "api/{controller}/{action}",
                 defaults: new { id = RouteParameter.Optional }
             );
             */

        }
    }
}
