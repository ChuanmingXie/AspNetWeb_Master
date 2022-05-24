using System.Web.Mvc;
using System.Web.Routing;
using Mvc5.Knowleadge.Infrastructure;

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

            //context.Routes.IgnoreRoute("Areas/Content/{filename}.html");

            context.MapRoute("DiskFile", "Areas/file/StaticContent.html", new { controller = "Customer", action = "List" });

            //context.MapRoute("MyRoute", "{controller}/{action}");

            //context.MapRoute("MyOtherRoute", "App/{action}", new { controller = "Home" });

            context.Routes.Add(new Route("SayHello", new CustomRouteHander()));

            context.Routes.Add(new LegacyRoute("~/RoutesHighAttribute/Legacy/GetLegacyURL", "~/old/Legacy/GetLegacyURL"));

            context.MapRoute("NewRoute", "App/Do{action}", new { controller = "Admin" });

            context.MapRoute(
                "RoutesHighAttribute_default",
                "RoutesHighAttribute/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}