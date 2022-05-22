﻿using System.Web.Mvc;

namespace Mvc5.Knowleadge.Areas.RoutesHighAttribute
{
    public class RoutesHighAttributeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RoutesHighAttribute";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapMvcAttributeRoutes();

            //context.MapRoute("MyRoute", "{controller}/{action}");

            //context.MapRoute("MyOtherRoute", "App/{action}", new { controller = "Home" });

            context.MapRoute("NewRoute", "App/Do{action}", new { controller = "Admin" });

            context.MapRoute(
                "RoutesHighAttribute_default",
                "RoutesHighAttribute/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}